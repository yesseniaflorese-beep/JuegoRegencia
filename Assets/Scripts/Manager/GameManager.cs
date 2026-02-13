using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public enum PlayerRoute
    {
        Hombre,
        Mujer
    }

public PlayerRoute selectedRoute;

    [Header("Stats")]
    public int amor=50;
    public int reputacion=80;
    public int dinero=70;

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
}

