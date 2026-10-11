using UnityEngine;


public class ReaparecerEnInicio : MonoBehaviour
{
    [SerializeField] private float limiteCaida = -8f;
    [SerializeField] private string tagTrampa = "Trampa";

    private Vector3 posicionInicial;
    private Rigidbody2D rb;

    private void Start()
    {
        posicionInicial = transform.position;
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (transform.position.y < limiteCaida) Reaparecer();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(tagTrampa)) Reaparecer();
    }

    private void Reaparecer()
    {
        transform.position = posicionInicial;
        if (rb != null) rb.linearVelocity = Vector2.zero;
    }
}
