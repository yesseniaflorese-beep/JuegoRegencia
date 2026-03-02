using UnityEngine;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public AudioSource musicSource;
    public AudioSource sfxSource;

    public List<string> musicNames;
    public List<AudioClip> musicClips;

    public List<string> sfxNames;
    public List<AudioClip> sfxClips;

    bool sonidoActivo = true;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // opcional pero recomendado
        }
        else
        {
            Destroy(gameObject);
        }
    }

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
        musicSource.mute = true;
        sfxSource.mute = true;
        sonidoActivo = false;
    }

    // 🔊 Activar sonido
    public void ActivarSonido()
    {
        musicSource.mute = false;
        sfxSource.mute = false;
        sonidoActivo = true;
    }

    // 🔁 Botón toggle (recomendado)
    public void ToggleSonido()
    {
        sonidoActivo = !sonidoActivo;

        musicSource.mute = !sonidoActivo;
        sfxSource.mute = !sonidoActivo;
    }

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