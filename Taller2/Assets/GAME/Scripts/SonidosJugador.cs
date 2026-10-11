using UnityEngine;

// Sonidos del jugador: salto, pasos (en bucle mientras camina), caída y golpe.
// Lee el estado de PlayerController; no modifica su lógica.
// Otros scripts pueden llamar a ReproducirCaida() y ReproducirGolpe().
[RequireComponent(typeof(PlayerController))]
[RequireComponent(typeof(Rigidbody2D))]
public class SonidosJugador : MonoBehaviour
{
    [Header("Clips")]
    [SerializeField] private AudioClip sonidoSalto;
    [SerializeField] private AudioClip sonidoPasos;
    [SerializeField] private AudioClip sonidoCaida;
    [SerializeField] private AudioClip sonidoGolpe;

    [Header("Volumen")]
    [SerializeField, Range(0f, 1f)] private float volumenSalto = 0.8f;
    [SerializeField, Range(0f, 1f)] private float volumenPasos = 0.5f;
    [SerializeField, Range(0f, 1f)] private float volumenCaida = 0.8f;
    [SerializeField, Range(0f, 1f)] private float volumenGolpe = 0.8f;

    private PlayerController controlador;
    private Rigidbody2D rb;
    private AudioSource fuenteEfectos;
    private AudioSource fuentePasos;

    private void Awake()
    {
        controlador = GetComponent<PlayerController>();
        rb = GetComponent<Rigidbody2D>();

        fuenteEfectos = CrearFuente();

        fuentePasos = CrearFuente();
        fuentePasos.clip = sonidoPasos;
        fuentePasos.loop = true;
        fuentePasos.volume = volumenPasos;
    }

    private AudioSource CrearFuente()
    {
        AudioSource fuente = gameObject.AddComponent<AudioSource>();
        fuente.playOnAwake = false;
        fuente.spatialBlend = 0f; // sonido 2D
        return fuente;
    }

    private void Update()
    {
        // Salto: misma condición que usa PlayerController.
        if (Input.GetKeyDown(KeyCode.Space) && controlador.EnSuelo && sonidoSalto != null)
        {
            fuenteEfectos.PlayOneShot(sonidoSalto, volumenSalto);
        }

        // Pasos: solo si está en el suelo y se mueve de lado.
        bool caminando = controlador.EnSuelo && Mathf.Abs(rb.linearVelocity.x) > 0.1f;

        if (caminando && !fuentePasos.isPlaying && sonidoPasos != null)
        {
            fuentePasos.Play();
        }
        else if (!caminando && fuentePasos.isPlaying)
        {
            fuentePasos.Stop();
        }
    }

    public void ReproducirCaida()
    {
        if (sonidoCaida != null) fuenteEfectos.PlayOneShot(sonidoCaida, volumenCaida);
    }

    public void ReproducirGolpe()
    {
        if (sonidoGolpe != null) fuenteEfectos.PlayOneShot(sonidoGolpe, volumenGolpe);
    }
}
