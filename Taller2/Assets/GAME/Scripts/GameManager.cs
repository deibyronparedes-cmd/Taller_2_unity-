using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public struct InventoryItem
{
    public string id;
    public string tipo;
    public escenaOrigen escena;
    public float timestamp;
}

[System.Serializable]
public struct DamageRecord
{
    public string causa; // "enemigo", "obstaculo", "jefe", "caida"
    public string escena;
    public float tiempo;
}

public enum escenaOrigen { Mina, Criatura }

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Metas de Minerales y Escena")]
    public int targetMineralA = 5;
    public int targetMineralB = 3;
    public string nextSceneName = "Criatura";

    [Header("Referencias a Config (Inyectadas desde el JSON)")]
    public string nombreJugador;
    public int vidasMaximas = 3;
    public float velocidadJugador = 5f;
    public float fuerzaSalto = 9f;
    public float tiempoInvulnerabilidad = 1.5f;

    [Header("Estructuras de Datos Obligatorias")]
    public Dictionary<string, int> inventarioConteo = new Dictionary<string, int>();
    public List<InventoryItem> inventarioDetallado = new List<InventoryItem>();
    public List<DamageRecord> historialGolpesMuertes = new List<DamageRecord>();
    public Stack<Vector3> stackCheckpoints = new Stack<Vector3>();
    public Queue<System.Action> colaEventos = new Queue<System.Action>();

    [Header("Control de Partida y Puntaje")]
    public int puntajeTotal = 0;
    public int muertesTotales = 0;
    public int golpesTotales = 0;
    public float tiempoMina = 0f;
    public float tiempoCriatura = 0f;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (colaEventos.Count > 0)
        {
            System.Action eventoSiguiente = colaEventos.Dequeue();
            eventoSiguiente?.Invoke();
        }

        if (SceneManager.GetActiveScene().name == "Mina")
        {
            tiempoMina += Time.deltaTime;
        }
        else if (SceneManager.GetActiveScene().name == "Criatura")
        {
            tiempoCriatura += Time.deltaTime;
        }
    }

    
    public void AddMineral(string tipoMineral)
    {
        int puntosAportados = 10;
        string idUnico = tipoMineral + "_" + System.Guid.NewGuid().ToString().Substring(0, 4);

        RegistrarRecoleccion(idUnico, tipoMineral, puntosAportados);
        VerificarCondicionElevador();
    }

    public void RegistrarRecoleccion(string idRecurso, string tipoRecurso, int puntos)
    {
        puntajeTotal += puntos;

        if (inventarioConteo.ContainsKey(tipoRecurso))
        {
            inventarioConteo[tipoRecurso]++;
        }
        else
        {
            inventarioConteo[tipoRecurso] = 1;
        }

        InventoryItem nuevoItem = new InventoryItem
        {
            id = idRecurso,
            tipo = tipoRecurso,
            escena = (SceneManager.GetActiveScene().name == "Mina") ? escenaOrigen.Mina : escenaOrigen.Criatura,
            timestamp = Time.time
        };
        inventarioDetallado.Add(nuevoItem);

        Debug.Log("Recurso registrado: " + idRecurso + " | Tipo: " + tipoRecurso);
    }

    private void VerificarCondicionElevador()
    {
        int cantA = inventarioConteo.ContainsKey("MineralA") ? inventarioConteo["MineralA"] : 0;
        int cantB = inventarioConteo.ContainsKey("MineralB") ? inventarioConteo["MineralB"] : 0;

        Debug.Log($"Mineral A: {cantA}/{targetMineralA} | Mineral B: {cantB}/{targetMineralB}");

        if (cantA >= targetMineralA && cantB >= targetMineralB)
        {
            Debug.Log("¡Requisitos cumplidos!");
        }
    }

    public void RegistrarCheckpoint(Vector3 nuevaPosicion)
    {
        stackCheckpoints.Push(nuevaPosicion);
        Debug.Log("Checkpoint apilado en la posición: " + nuevaPosicion);
    }

    public Vector3 ObtenerUltimoCheckpoint(Vector3 posicionInicialPorDefecto)
    {
        if (stackCheckpoints.Count > 0)
        {
            return stackCheckpoints.Peek();
        }
        return posicionInicialPorDefecto;
    }

    public void RegistrarDanioOMuerte(string causa, bool esMuerte)
    {
        if (esMuerte) muertesTotales++;
        else golpesTotales++;

        DamageRecord registro = new DamageRecord
        {
            causa = causa,
            escena = SceneManager.GetActiveScene().name,
            tiempo = Time.time
        };
        historialGolpesMuertes.Add(registro);
    }
}