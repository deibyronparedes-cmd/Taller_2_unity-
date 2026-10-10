using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Lógica exclusiva de la escena Menu.
// No crea GameManager: solo lee config.json para mostrar saludo o error.
public class MenuController : MonoBehaviour
{
    [Header("Textos")]
    [SerializeField] private TMP_Text textoSaludo;
    [SerializeField] private TMP_Text textoError;

    [Header("Botones y paneles")]
    [SerializeField] private Button botonJugar;
    [SerializeField] private GameObject panelInstrucciones;

    [Header("Escenas")]
    [SerializeField] private string escenaJuego = "Mina";

    private void Start()
    {
        if (panelInstrucciones != null) panelInstrucciones.SetActive(false);
        if (textoError != null) textoError.gameObject.SetActive(false);

        string error;
        ConfigData config = JsonService.CargarConfig(out error);

        if (config == null)
        {
            // El juego no se cierra: se muestra el error en pantalla.
            if (textoError != null)
            {
                textoError.text = error;
                textoError.gameObject.SetActive(true);
            }
            if (textoSaludo != null) textoSaludo.text = string.Empty;
            if (botonJugar != null) botonJugar.interactable = false;
            return;
        }

        if (textoSaludo != null)
            textoSaludo.text = "¡Bienvenido, " + config.jugador.nombre + "!";
        if (botonJugar != null) botonJugar.interactable = true;
    }

    // --- Métodos para enlazar en el OnClick de los botones ---

    public void Jugar()
    {
        SceneManager.LoadScene(escenaJuego);
    }

    public void AbrirInstrucciones()
    {
        if (panelInstrucciones != null) panelInstrucciones.SetActive(true);
    }

    public void CerrarInstrucciones()
    {
        if (panelInstrucciones != null) panelInstrucciones.SetActive(false);
    }

    public void Salir()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
