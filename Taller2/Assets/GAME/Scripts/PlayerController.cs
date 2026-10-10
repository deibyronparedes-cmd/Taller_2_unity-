using UnityEngine;

// Movimiento horizontal y salto del jugador.
// Velocidad y fuerza de salto vienen de config.json (jugador.velocidad, jugador.fuerzaSalto).
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [Header("Detección de suelo (Raycast2D)")]
    [SerializeField] private Transform puntoSuelo;
    [SerializeField] private float distanciaSuelo = 0.15f;
    [SerializeField] private LayerMask capaSuelo;

    [Header("Animación (opcional)")]
    [SerializeField] private Animator animator;

    private Rigidbody2D rb;
    private float velocidad;
    private float fuerzaSalto;
    private bool configLista;

    // Para las mejoras temporales de los minerales (se usarán más adelante).
    public float MultiplicadorVelocidad { get; set; } = 1f;
    public float MultiplicadorSalto { get; set; } = 1f;

    public bool EnSuelo { get; private set; }
    public int Direccion { get; private set; } = 1; // 1 = derecha, -1 = izquierda

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

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

        // Raycast hacia abajo: solo se puede saltar si detecta suelo.
        RaycastHit2D golpe = Physics2D.Raycast(puntoSuelo.position, Vector2.down, distanciaSuelo, capaSuelo);
        EnSuelo = golpe.collider != null;

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

    // Dibuja el rayo del suelo en la vista Scene para poder ajustarlo.
    private void OnDrawGizmosSelected()
    {
        if (puntoSuelo == null) return;
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(puntoSuelo.position, puntoSuelo.position + Vector3.down * distanciaSuelo);
    }
}
