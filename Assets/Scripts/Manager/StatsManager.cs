using UnityEngine;

public class StatsManager : MonoBehaviour
{
    public static StatsManager instance;

    [Header("Stats")]
    public int amor = 0;
    public int reputacion = 0;
    public int dinero = 0;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    public void ModifyStat(string stat, int amount)
    {
        switch (stat.ToLower())
        {
            case "amor":
                amor += amount;
                break;

            case "reputacion":
                reputacion += amount;
                break;

            case "dinero":
                dinero += amount;
                break;
        }

        Debug.Log($"📊 {stat}: {amount} → Nuevo valor: {GetStat(stat)}");
    }

    public int GetStat(string stat)
    {
        switch (stat.ToLower())
        {
            case "amor": return amor;
            case "reputacion": return reputacion;
            case "dinero": return dinero;
        }
        return 0;
    }
}
