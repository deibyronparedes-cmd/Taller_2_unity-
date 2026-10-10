using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    [Header("Textos")]
    [SerializeField] private TMP_Text textoSaludo;
    [SerializeField] private TMP_Text textoError;

    [Header("Botones")]
    [SerializeField] private Button botonJugar;

    [Header("Escenas")]
    [SerializeField] private string escenaJuego = "Mina";
    [SerializeField] private string escenaInstrucciones = "Instrucciones";

    private void Start()
    {
        if (textoError != null) textoError.gameObject.SetActive(false);

        string error;
        ConfigData config = JsonService.CargarConfig(out error);

        if (config == null)
        {
            .
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

    

    public void Jugar()
    {
        SceneManager.LoadScene(escenaJuego);
    }

    public void AbrirInstrucciones()
    {
        SceneManager.LoadScene(escenaInstrucciones);
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
