using UnityEngine;
using UnityEngine.SceneManagement;

// Lógica exclusiva de la escena Instrucciones.
public class InstruccionesController : MonoBehaviour
{
    [SerializeField] private string escenaMenu = "Menu";

    // Enlazar en el OnClick del botón Volver.
    public void Volver()
    {
        SceneManager.LoadScene(escenaMenu);
    }
}
