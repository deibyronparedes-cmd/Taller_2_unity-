using System.Collections;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    private int vidasActuales;
    private int vidasMaximas = 3;
    private float tiempoInvulnerabilidad = 1.5f;
    private bool esInvulnerable = false;

    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rb;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();

        if (GameManager.Instance != null)
        {
            vidasMaximas = GameManager.Instance.vidasMaximas;
            tiempoInvulnerabilidad = GameManager.Instance.tiempoInvulnerabilidad;
        }

        vidasActuales = vidasMaximas;
        Debug.Log("Vidas iniciales del jugador: " + vidasActuales);
    }

    public void RecibirDanio(int cantidadDanio, string causa)
    {
        if (esInvulnerable) return;

        vidasActuales -= cantidadDanio;
        Debug.Log($"¡Daño recibido! Vidas restantes: {vidasActuales}. Causa: {causa}");

 
        if (GameManager.Instance != null)
        {
            GameManager.Instance.RegistrarDanioOMuerte(causa, false);
        }

        if (vidasActuales <= 0)
        {
            Morir(causa);
        }
        else
        {
            StartCoroutine(ActivarInvulnerabilidadRoutine());
        }
    }

    private IEnumerator ActivarInvulnerabilidadRoutine()
    {
        esInvulnerable = true;
        float tiempoTranscurrido = 0f;

        while (tiempoTranscurrido < tiempoInvulnerabilidad)
        {
            if (spriteRenderer != null)
            {
                Color color = spriteRenderer.color;
                color.a = (color.a == 1f) ? 0.3f : 1f;
                spriteRenderer.color = color;
            }

            yield return new WaitForSeconds(0.15f);
            tiempoTranscurrido += 0.15f;
        }

        if (spriteRenderer != null)
        {
            Color color = spriteRenderer.color;
            color.a = 1f;
            spriteRenderer.color = color;
        }

        esInvulnerable = false;
    }

    private void Morir(string causa)
    {
        Debug.Log("El jugador ha muerto por: " + causa);

        if (GameManager.Instance != null)
        {
            GameManager.Instance.RegistrarDanioOMuerte(causa, true);
        }

        UnityEngine.SceneManagement.SceneManager.LoadScene(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene().name
        );
    }
}