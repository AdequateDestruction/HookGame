using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManagerScript : MonoBehaviour
{
    // Add tracks here as needed
    public AudioClip startingTrack, endingTrack, victoryTrack, deathTrack, mainMenuTrack;
    public AudioSource musicSource;

    static MusicManagerScript instance = null;
    float preFadeVolume = 0.6f;
    bool fading;

    // Setting a static instance and checking it for DontDestroyOnLoad purposes
    public static MusicManagerScript Instance
    {
        get { return instance; }
    }

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            instance = this;
        }
        DontDestroyOnLoad(this.gameObject);
    }

    void Start()
    {
        musicSource.clip = startingTrack;
        musicSource.Play();
    }

    void Update()
    {
        if (fading)
        {
            musicSource.volume -= 0.2f * Time.deltaTime;
        }
    }

    public void SwitchToStage3Track()
    {
        musicSource.clip = endingTrack;
        musicSource.Play();
        musicSource.loop = true;
    }

    public void SwitchToVictoryTrack()
    {
        musicSource.clip = victoryTrack;
        musicSource.Play();
        musicSource.loop = false;
    }

    public void SwitchToDeathTrack()
    {
        musicSource.clip = deathTrack;
        musicSource.PlayDelayed(1.2f);
        musicSource.loop = false;
    }

    public void StartStage1Music()
    {
        fading = false;
        musicSource.volume = preFadeVolume;
        musicSource.clip = startingTrack;
        musicSource.Play();
        musicSource.loop = true;
    }

    public void StartMainMenuMusic()
    {
        fading = false;
        musicSource.volume = preFadeVolume;
        musicSource.clip = mainMenuTrack;
        musicSource.Play();
        musicSource.loop = true;
    }

    public void FadeOutMusic()
    {
        preFadeVolume = musicSource.volume;
        fading = true;
    }
}
