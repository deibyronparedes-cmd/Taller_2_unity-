using UnityEngine;

public class Collectible : MonoBehaviour
{
    public string mineralType = "MineralA";

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            
            GameManager.Instance.AddMineral(mineralType);

            
            Destroy(gameObject);
        }
    }
}