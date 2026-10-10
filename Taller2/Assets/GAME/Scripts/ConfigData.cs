using System;
using System.Collections.Generic;



[Serializable]
public class JugadorConfig
{
    public string nombre;
    public int vidas;
    public float velocidad;
    public float fuerzaSalto;
    public float invulnerabilidad;
}

[Serializable]
public class RequisitoConfig
{
    public string tipo;
    public int cantidad;
}

[Serializable]
public class RecursoConfig
{
    public string id;
    public string tipo;
    public int puntos;
    public string efecto;
    public float valor;
    public float duracion;
}

[Serializable]
public class PeligroConfig
{
    public string id;
    public string tipo;
    public int dano;
    public float velocidad;
}

[Serializable]
public class JefeConfig
{
    public int vida;
    public List<int> umbralesFase;
    public List<float> velocidadPorFase;
    public List<int> danoPorFase;
    public int puntosVictoria;
}

[Serializable]
public class ConfigData
{
    public JugadorConfig jugador;
    public List<RequisitoConfig> requisitoJefe;
    public List<RecursoConfig> recursos;
    public List<PeligroConfig> peligros;
    public JefeConfig jefe;

    public RecursoConfig GetRecurso(string id)
    {
        return recursos.Find(r => r.id == id);
    }

    public PeligroConfig GetPeligro(string id)
    {
        return peligros.Find(p => p.id == id);
    }
}
