using UnityEngine;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [Header("Audio Sources")]
    public AudioSource musicSource;
    public AudioSource sfxSource;

    [Header("Music")]
    public List<string> musicNames;
    public List<AudioClip> musicClips;

    [Header("SFX")]
    public List<string> sfxNames;
    public List<AudioClip> sfxClips;

    bool sonidoActivo = true;

    void Awake()
    {
        // Singleton
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        // Cargar estado guardado del sonido
        sonidoActivo = PlayerPrefs.GetInt("Muted", 1) == 1;

        AudioListener.volume = sonidoActivo ? 1f : 0f;
    }

    // =========================
    // 🎵 REPRODUCIR MÚSICA
    // =========================
public void PlayMusic(string name)
{
    for (int i = 0; i < musicNames.Count; i++)
    {
        if (musicNames[i].ToLower() == name.ToLower())
        {
            // Evita reiniciar misma canción
            if (musicSource.clip == musicClips[i] && musicSource.isPlaying)
                return;

            // Detener música actual
            musicSource.Stop();

            // Cambiar clip
            musicSource.clip = musicClips[i];

            // Reproducir nueva música
            musicSource.Play();

            return;
        }
    }

    Debug.LogWarning("❌ Música no encontrada: " + name);
}

    // =========================
    // 🔊 REPRODUCIR SFX
    // =========================
    public void PlaySFX(string name)
    {
        for (int i = 0; i < sfxNames.Count; i++)
        {
            if (sfxNames[i].ToLower() == name.ToLower())
            {
                sfxSource.PlayOneShot(sfxClips[i]);
                return;
            }
        }

        Debug.LogWarning("❌ SFX no encontrado: " + name);
    }

    // =========================
    // 🔁 TOGGLE SONIDO
    // =========================
    public void ToggleSonido()
    {
        sonidoActivo = !sonidoActivo;

        AudioListener.volume = sonidoActivo ? 1f : 0f;

        // Guardar estado
        PlayerPrefs.SetInt("Muted", sonidoActivo ? 1 : 0);
        PlayerPrefs.Save();
    }

    // =========================
    // 🔍 ESTADO SONIDO
    // =========================
    public bool SonidoActivo()
    {
        return sonidoActivo;
    }

    // =========================
    // 🎚 VOLUMEN MÚSICA
    // =========================
    public void SetVolumenMusica(float valor)
    {
        musicSource.volume = valor;
    }

    // =========================
    // 🎚 VOLUMEN SFX
    // =========================
    public void SetVolumenSFX(float valor)
    {
        sfxSource.volume = valor;
    }
}