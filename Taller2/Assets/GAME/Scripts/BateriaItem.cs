using UnityEngine;

public class BateriaItem : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (GameManager.Instance != null)
            {
                if (GameManager.Instance.inventarioConteo.ContainsKey("bateria"))
                {
                    GameManager.Instance.inventarioConteo["bateria"]++;
                }
                else
                {
                    GameManager.Instance.inventarioConteo.Add("bateria", 1);
                }
                Debug.Log("¡Batería recogida! Total: " + GameManager.Instance.inventarioConteo["bateria"]);
            }
            Destroy(gameObject);
        }
    }
}