using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Metas necesarias para ganar")]
    public int targetMineralA = 5; 
    public int targetMineralB = 3; 

    [Header("Contadores actualizados")]
    private int currentMineralA = 0;
    private int currentMineralB = 0;

    [Header("Nombre de la siguiente escena")]
    public string nextSceneName = "NombreDeLaSiguienteEscena";

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddMineral(string type)
    {
        if (type == "MineralA")
        {
            currentMineralA++;
            Debug.Log("Mineral A recolectado: " + currentMineralA + "/" + targetMineralA);
        }
        else if (type == "MineralB")
        {
            currentMineralB++;
            Debug.Log("Mineral B recolectado: " + currentMineralB + "/" + targetMineralB);
        }

        // Comprobamos si ya alcanzamos las metas de ambos minerales
        CheckWinCondition();
    }

    private void CheckWinCondition()
    {
        if (currentMineralA >= targetMineralA && currentMineralB >= targetMineralB)
        {
            Debug.Log("¡Meta alcanzada! Cambiando de escena...");
            LoadNextScene();
        }
    }

    private void LoadNextScene()
    {
        SceneManager.LoadScene(nextSceneName);
    }
}