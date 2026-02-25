using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public enum PlayerRoute
    {
        Mujer = 0,
        Hombre = 1
    }

    [Header("Ruta Inicial")]
    public PlayerRoute selectedRoute;

    [Header("Stats")]
    public int amor = 0;
    public int reputacion = 0;
    public int dinero = 0;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // ============================
    // SUMAR STATS
    // ============================
    public void AddStat(string stat, int value)
    {
        switch (stat.ToLower())
        {
            case "amor":
                amor += value;
                break;

            case "reputacion":
                reputacion += value;
                break;

            case "dinero":
                dinero += value;
                break;
        }

        Debug.Log($"📊 {stat} ahora es {GetStat(stat)}");
    }

    // ============================
    // OBTENER STATS
    // ============================
    public int GetStat(string stat)
    {
        switch (stat.ToLower())
        {
            case "amor": return amor;
            case "reputacion": return reputacion;
            case "dinero": return dinero;

            // 👇 NECESARIO PARA @IF genero ==
            case "genero": return (int)selectedRoute;
        }

        return 0;
    }

    // ============================
    // EVALUAR RUTA FINAL
    // ============================
    public string GetDominantRoute()
    {
        if (amor >= reputacion && amor >= dinero)
            return "amor";

        if (reputacion >= amor && reputacion >= dinero)
            return "reputacion";

        return "dinero";
    }
}
