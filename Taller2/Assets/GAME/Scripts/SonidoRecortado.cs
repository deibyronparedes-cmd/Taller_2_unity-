using UnityEngine;

// Reproduce solo un fragmento de un AudioClip (desde "inicio", durante "duracion" segundos).
// Se enlaza en el OnClick de un botón: SonidoRecortado > Reproducir().
[RequireComponent(typeof(AudioSource))]
public class SonidoRecortado : MonoBehaviour
{
    [Header("Fragmento a reproducir (segundos)")]
    [SerializeField] private float inicio = 0f;
    [SerializeField] private float duracion = 0.3f;

    private AudioSource fuente;

    private void Awake()
    {
        fuente = GetComponent<AudioSource>();
        fuente.playOnAwake = false;
        fuente.loop = false;
    }

    public void Reproducir()
    {
        if (fuente.clip == null) return;

        CancelInvoke(nameof(Detener));
        fuente.Stop();
        fuente.time = Mathf.Clamp(inicio, 0f, fuente.clip.length - 0.01f);
        fuente.Play();
        Invoke(nameof(Detener), duracion);
    }

    private void Detener()
    {
        fuente.Stop();
    }
}
