using UnityEngine;
using UnityEngine.SceneManagement;

public class DamageDealer : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("¡El jugador tocó un pincho!");

            //Destroy(collision.gameObject);

            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}