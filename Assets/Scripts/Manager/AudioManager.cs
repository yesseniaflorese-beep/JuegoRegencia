using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

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

    [Header("UI Sonido")]
    public Image iconoSonido;   // Imagen del botón
    public Sprite iconoActivo;  // 🔊
    public Sprite iconoMute;    // 🔇

    bool sonidoActivo = true;

    void Awake()
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

    void Start()
    {
        ActualizarIcono(); // asegura que el icono esté bien al iniciar
    }

    // 🎵 Reproducir música
    public void PlayMusic(string name)
    {
        for (int i = 0; i < musicNames.Count; i++)
        {
            if (musicNames[i].ToLower() == name.ToLower())
            {
                musicSource.clip = musicClips[i];
                musicSource.Play();
                return;
            }
        }
    }

    // 🔊 Reproducir SFX
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
    }

    // 🔇 Mutear todo
    public void MutearTodo()
    {
        sonidoActivo = false;
        musicSource.mute = true;
        sfxSource.mute = true;

        ActualizarIcono();
    }

    // 🔊 Activar sonido
    public void ActivarSonido()
    {
        sonidoActivo = true;
        musicSource.mute = false;
        sfxSource.mute = false;

        ActualizarIcono();
    }

    // 🔁 Toggle desde botón
public void ToggleSonido()
{
    sonidoActivo = !sonidoActivo;

    if (sonidoActivo)
    {
        AudioListener.volume = 1f;
    }
    else
    {
        AudioListener.volume = 0f;
    }

    ActualizarIcono();
}

    // 🔄 Cambiar icono
    void ActualizarIcono()
    {
        if (iconoSonido == null) return;

        if (sonidoActivo)
        {
            iconoSonido.sprite = iconoActivo;
        }
        else
        {
            iconoSonido.sprite = iconoMute;
        }
    }

    // 🎚 Volumen
    public void SetVolumenMusica(float valor)
    {
        Debug.Log("Cambiando volumen a: " + valor);
        musicSource.volume = valor;
    }

    public void SetVolumenSFX(float valor)
    {
        sfxSource.volume = valor;
    }
}