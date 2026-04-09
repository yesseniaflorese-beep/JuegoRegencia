using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    // 🔥 Evento para notificar cambio de stats
    public delegate void OnStatChanged(string statName);
    public static event OnStatChanged onStatChanged;

    // public enum PlayerRoute
    // {
    //     Mujer = 0,
    //     Hombre = 1
    // }

    [Header("Ruta Inicial")]
    // public PlayerRoute selectedRoute;

    [Header("Stats")]
    public int amor = 0;
    public int reputacion = 0;
    public int dinero = 0;

    public int ambicion = 0;
    public int theoPoints = 0;
    public int sebastianPoints = 0;
    public int routeTheo = 0;
    public int routeSebastian = 0;
    

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
    // SUMAR / RESTAR STATS
    // ============================
    public void AddStat(string stat, int value)
    {
        stat = stat.ToLower();

        switch (stat)
        {
            case "amor":
                amor = Mathf.Clamp(amor + value, 0, 100);
                break;

            case "reputacion":
                reputacion = Mathf.Clamp(reputacion + value, 0, 100);
                break;

            case "dinero":
                dinero = Mathf.Clamp(dinero + value, 0, 100);
                break;

            case "ambicion":
                ambicion = Mathf.Clamp(ambicion + value, 0, 100);
                break;

             case "theopoints":
                theoPoints = Mathf.Clamp(theoPoints + value, 0, 100);
                break;

             case "sebastianpoints":
                sebastianPoints = Mathf.Clamp(sebastianPoints + value, 0, 100);
                break;

            case "routetheo":
                routeTheo += value;
                break;

            case "routesebastian":
                routeSebastian += value;
                break;

            default:
                Debug.LogWarning("⚠ Stat no reconocido: " + stat);
                return;
        }

        // 🔥 Notifica a las barras que cambió el stat
        onStatChanged?.Invoke(stat);

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
            case "ambicion": return ambicion;
            case "theopoints": return theoPoints;
            case "sebastianpoints": return sebastianPoints;


            case "routetheo": return routeTheo;
            case "routesebastian": return routeSebastian;

            // 👇 Para condiciones tipo @IF genero ==
            //case "genero": return (int)selectedRoute;
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