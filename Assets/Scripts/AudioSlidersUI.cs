using UnityEngine;
using UnityEngine.UI;

public class AudioSlidersUI : MonoBehaviour
{
    public Slider musicSlider;
    public Slider sfxSlider;

    void Start()
    {
        // Valores iniciales
        musicSlider.value = AudioManager.instance.musicSource.volume;
        sfxSlider.value = AudioManager.instance.sfxSource.volume;

        // Eventos
        musicSlider.onValueChanged.AddListener(SetMusicVolume);
        sfxSlider.onValueChanged.AddListener(SetSFXVolume);
    }

    void SetMusicVolume(float value)
    {
        AudioManager.instance.SetVolumenMusica(value);
    }

    void SetSFXVolume(float value)
    {
        AudioManager.instance.SetVolumenSFX(value);
    }
}