using System;
using System.IO;
using UnityEngine;


public static class JsonService
{
    public const string NombreConfig = "config.json";
    public const string NombreResumen = "resumen_partida.json";

    
    public static ConfigData CargarConfig(out string error)
    {
        error = null;
        string ruta = Path.Combine(Application.streamingAssetsPath, NombreConfig);

        try
        {
            if (!File.Exists(ruta))
            {
                error = "No se encontró config.json en: " + ruta;
                return null;
            }

            string json = File.ReadAllText(ruta);
            ConfigData config = JsonUtility.FromJson<ConfigData>(json);

            if (config == null || config.jugador == null || config.jefe == null ||
                config.recursos == null || config.peligros == null || config.requisitoJefe == null)
            {
                error = "config.json está dañado o incompleto.";
                return null;
            }

            return config;
        }
        catch (Exception e)
        {
            error = "Error al leer config.json: " + e.Message;
            return null;
        }
    }

    public static string RutaResumen()
    {
        return Path.Combine(Application.persistentDataPath, NombreResumen);
    }
}
