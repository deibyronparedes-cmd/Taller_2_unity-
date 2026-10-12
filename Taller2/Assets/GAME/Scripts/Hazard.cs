using UnityEngine;

public class Hazard : MonoBehaviour
{
    [Header("Configuración del Peligro")]
    public int danio = 1; 
    public string causaMuerte = "obstaculo";

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerHealth health = collision.gameObject.GetComponent<PlayerHealth>();
            if (health != null)
            {
                health.RecibirDanio(danio, causaMuerte);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerHealth health = collision.GetComponent<PlayerHealth>();
            if (health != null)
            {
                health.RecibirDanio(danio, causaMuerte);
            }
        }
    }
}