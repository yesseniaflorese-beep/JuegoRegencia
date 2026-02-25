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

    void Awake()
    {
        instance = this;
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
}