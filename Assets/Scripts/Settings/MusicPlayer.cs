using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
   
    private AudioSource audioSource;
    private bool _transitioning = false;
    public float volumeTransitionSpeed = 4f; // Speed at which volume changes
    public AudioSource AudioSource
    {
        get
        {
            if(audioSource == null)
            {
                audioSource = GetComponent<AudioSource>();
            }
            return audioSource;
        } 
    }
    private static MusicPlayer _singleton;
    public static MusicPlayer Singleton
    {
        get
        {
            if (_singleton == null)
            {
                // Load the MusicPlayer prefab from Resources
                GameObject musicPlayerPrefab = Resources.Load<GameObject>("MusicPlayer");
                if (musicPlayerPrefab != null)
                {
                    GameObject musicPlayerInstance = Instantiate(musicPlayerPrefab);
                    Singleton = musicPlayerInstance.GetComponent<MusicPlayer>();
                    //Debug.Log("MusicPlayer Instantiated");
                }
                else
                {
                    Debug.LogError("MusicPlayer prefab not found in Resources.");
                }
            }
            return _singleton;
        }
        private set
        {
            if (_singleton == null)
            {
                _singleton = value;
            }
            else if (_singleton != value)
            {
                Debug.LogWarning($"{nameof(MusicPlayer)} instance already exists. Destroying duplicate!");
                Destroy(value.gameObject);
            }
        }
    }
    void Awake()
    {
        if (_singleton == null)
        {
            Singleton = this;
            audioSource = GetComponent<AudioSource>();
            DontDestroyOnLoad(this.gameObject);
        }
        else if (_singleton != this)
        {
            Destroy(this.gameObject);
        }
    }


    public void PlaySong(string songName, bool loops = false)
    {

        if (!string.IsNullOrEmpty(songName))
        {
            // Load the sprite from Resources folder
            string fullPath = "Soundtrack/" + songName; // Assuming the path is relative to the Resources folder

            AudioClip Song = Resources.Load(fullPath) as AudioClip;
            if(Song != null)
            {
                PlaySong(Song, loops);
            }
        }
    }

    public static void Stop()
    {
        Singleton.audioSource.Stop();
    }

    public static void Resume()
    {
        Singleton.audioSource.Play();
    }

    public void PlaySong(AudioClip song, bool loops = false)
    {
        StartCoroutine(TransitioningSong(song, loops));
    }

    public void StopCurrentSong()
    {
        audioSource.Stop();
    }

    private IEnumerator TransitioningSong(AudioClip song, bool loops = false)
    {
        if(audioSource.clip != song)
        {

        while (_transitioning)
        {
            yield return null;
        }

        _transitioning = true;

        // Gradually lower the volume
        while (audioSource.volume > 0)
        {
            audioSource.volume -= volumeTransitionSpeed * Time.deltaTime;
            yield return null;
        }

        // Change the clip and play the new song
        audioSource.Stop();
        audioSource.clip = song;
        audioSource.loop = loops;
        audioSource.Play();

        // Gradually raise the volume
        while (audioSource.volume < 1)
        {
            audioSource.volume += volumeTransitionSpeed * Time.deltaTime;
            yield return null;
        }

        audioSource.volume = 1; // Ensure the volume is exactly 1
        _transitioning = false;
        }
    }



}
