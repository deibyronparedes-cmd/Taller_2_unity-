using UnityEngine;
using UnityEngine.SceneManagement;


public class InstruccionesController : MonoBehaviour
{
    [SerializeField] private string escenaMenu = "Menu";

    
    public void Volver()
    {
        SceneManager.LoadScene(escenaMenu);
    }
}
