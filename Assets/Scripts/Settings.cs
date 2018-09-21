using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class Settings : MonoBehaviour
{
    // Currently just settings for audio volume
    // Change the volume == values if necessary

    public AudioSource sfxTestSource;
    public AudioMixer audioMixer;
    
    public void SetMusicVolume(float volume)
    {
        if (volume == -30f)
        {
            volume = -80f;
        }
        audioMixer.SetFloat("MusicVolume", volume);
    }

    public void SetSoundVolume(float volume)
    {
        if (volume == -30f)
        {
            volume = -80f;
        }
        sfxTestSource.Play();
        audioMixer.SetFloat("SoundVolume", volume);
    }
}
