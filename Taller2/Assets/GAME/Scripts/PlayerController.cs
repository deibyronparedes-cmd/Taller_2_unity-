using UnityEngine;


[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class PlayerController : MonoBehaviour
{
    [Header("Detección de suelo (Raycast2D)")]
    [Tooltip("Si se deja vacío, detecta cualquier layer menos la del propio jugador.")]
    [SerializeField] private LayerMask capaSuelo;
    [SerializeField] private float distanciaSuelo = 0.2f;

    [Header("Animación (opcional)")]
    [SerializeField] private Animator animator;

    private Rigidbody2D rb;
    private Collider2D colision;
    private readonly RaycastHit2D[] resultados = new RaycastHit2D[4];
    private float velocidad;
    private float fuerzaSalto;
    private bool configLista;

   
    public float MultiplicadorVelocidad { get; set; } = 1f;
    public float MultiplicadorSalto { get; set; } = 1f;

    public bool EnSuelo { get; private set; }
    public int Direccion { get; private set; } = 1; 

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        colision = GetComponent<Collider2D>();

        string error;
        ConfigData config = JsonService.CargarConfig(out error);
        if (config == null)
        {
            Debug.LogError("PlayerController: no se pudo leer la configuración. " + error);
            return;
        }

        velocidad = config.jugador.velocidad;
        fuerzaSalto = config.jugador.fuerzaSalto;
        configLista = true;
    }

    private void Update()
    {
        if (!configLista) return;

        EnSuelo = DetectarSuelo();

        float horizontal = Input.GetAxisRaw("Horizontal");
        rb.linearVelocity = new Vector2(horizontal * velocidad * MultiplicadorVelocidad, rb.linearVelocity.y);

        if (horizontal > 0.01f) Direccion = 1;
        else if (horizontal < -0.01f) Direccion = -1;
        Voltear();

        if (Input.GetKeyDown(KeyCode.Space) && EnSuelo)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, fuerzaSalto * MultiplicadorSalto);
        }

        ActualizarAnimator(horizontal);
    }

   
    private bool DetectarSuelo()
    {
        Bounds b = colision.bounds;
        Vector2 origen = new Vector2(b.center.x, b.min.y + 0.05f);

        ContactFilter2D filtro = new ContactFilter2D();
        filtro.useTriggers = false; 
        LayerMask mascara = capaSuelo.value == 0 ? (LayerMask)(~(1 << gameObject.layer)) : capaSuelo;
        filtro.SetLayerMask(mascara);

        int golpes = Physics2D.Raycast(origen, Vector2.down, filtro, resultados, distanciaSuelo + 0.05f);
        for (int i = 0; i < golpes; i++)
        {
            if (resultados[i].collider != colision) return true;
        }
        return false;
    }

    private void Voltear()
    {
        Vector3 escala = transform.localScale;
        escala.x = Mathf.Abs(escala.x) * Direccion;
        transform.localScale = escala;
    }

    private void ActualizarAnimator(float horizontal)
    {
        if (animator == null) return;
        animator.SetFloat("Velocidad", Mathf.Abs(horizontal));
        animator.SetBool("EnSuelo", EnSuelo);
    }

    
    private void OnDrawGizmosSelected()
    {
        Collider2D c = GetComponent<Collider2D>();
        if (c == null) return;
        Bounds b = c.bounds;
        Vector3 origen = new Vector3(b.center.x, b.min.y + 0.05f, 0f);
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(origen, origen + Vector3.down * (distanciaSuelo + 0.05f));
    }
}
