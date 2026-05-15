using UnityEngine;

public class SceneMusic : MonoBehaviour
{
    public string nombreMusica;

    void Start()
    {
        AudioManager.instance.PlayMusic(nombreMusica);
    }
}