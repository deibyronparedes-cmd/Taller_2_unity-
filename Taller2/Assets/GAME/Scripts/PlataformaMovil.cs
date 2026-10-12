using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlataformaMovil : MonoBehaviour
{
    [Header("Puntos de Movimiento")]
    public Transform puntoA;
    public Transform puntoB;
    public float velocidad = 2f;

    private Vector3 posicionA;
    private Vector3 posicionB;
    private Vector3 destinoActual;

    [Header("Activación por Batería (Requisito de Guía)")]
    public bool requiereBateria = false;
    private bool estaActivada = false;

    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Kinematic; 

        if (puntoA != null && puntoB != null)
        {
           
            posicionA = puntoA.position;
            posicionB = puntoB.position;

            transform.position = posicionA;
            destinoActual = posicionB;
        }
        else
        {
            Debug.LogError("¡Faltan asignar el Punto A o el Punto B en la Plataforma Móvil!");
        }

        if (!requiereBateria)
        {
            estaActivada = true;
        }
    }

    private void FixedUpdate()
    {
        if (!estaActivada || puntoA == null || puntoB == null) return;

        
        Vector3 nuevaPosicion = Vector3.MoveTowards(transform.position, destinoActual, velocidad * Time.fixedDeltaTime);
        rb.MovePosition(nuevaPosicion);

        if (Vector3.Distance(transform.position, destinoActual) < 0.1f)
        {
            destinoActual = (destinoActual == posicionA) ? posicionB : posicionA;
        }
    }

    public void ActivarPlataformaConBateria()
    {
        if (requiereBateria && !estaActivada)
        {
            estaActivada = true;
            Debug.Log("¡Plataforma activada con batería!");
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.transform.SetParent(transform);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.transform.SetParent(null);
        }
    }
}