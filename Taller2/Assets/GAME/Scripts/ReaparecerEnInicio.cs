using UnityEngine;

// Solo para la escena de demostración (Instrucciones):
// si el jugador cae al vacío o toca una trampa, vuelve a su posición inicial.
// En Mina y Criatura el respawn usa los checkpoints (Stack) del GameManager.
public class ReaparecerEnInicio : MonoBehaviour
{
    [SerializeField] private float limiteCaida = -8f;
    [SerializeField] private string tagTrampa = "Trampa";

    private Vector3 posicionInicial;
    private Rigidbody2D rb;
    private SonidosJugador sonidos;

    private void Start()
    {
        posicionInicial = transform.position;
        rb = GetComponent<Rigidbody2D>();
        sonidos = GetComponent<SonidosJugador>();
    }

    private void Update()
    {
        if (transform.position.y < limiteCaida)
        {
            if (sonidos != null) sonidos.ReproducirCaida();
            Reaparecer();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(tagTrampa))
        {
            if (sonidos != null) sonidos.ReproducirGolpe();
            Reaparecer();
        }
    }

    private void Reaparecer()
    {
        transform.position = posicionInicial;
        if (rb != null) rb.linearVelocity = Vector2.zero;
    }
}
