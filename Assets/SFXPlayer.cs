using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXPlayer : MonoBehaviour
{

    public static SFXPlayer instance;

    [SerializeField] AudioSource sfxSource;
    [SerializeField] AudioSource voiceSource;
    [SerializeField] AudioSource musicSource;

    [SerializeField] AudioClip[] navigate;
    [SerializeField] AudioClip[] select;
    [SerializeField] AudioClip[] cancel;
    [SerializeField] AudioClip[] voiceClips;
    [SerializeField] AudioClip[] sfxs;
    [SerializeField] AudioClip[] melodies;


    private void Awake()
    {
        instance = this;
    }
    private void PlayRandomClip(AudioSource source, AudioClip[] clips)
    {
        if (clips != null && clips.Length > 0)
        {
            AudioClip clip = clips[Random.Range(0, clips.Length)];
            source.PlayOneShot(clip);
        }
    }

    public void PlayOnNavigate()
    {
        PlayRandomClip(sfxSource, navigate);
    }

    public void PlayOnSelect()
    {
        PlayRandomClip(sfxSource, select);
    }

    public void PlayOnBack()
    {
        PlayRandomClip(sfxSource, cancel);
    }

    public void PlayOnVolumeChange()
    {
        PlayRandomClip(sfxSource, sfxs);
    }

    public void PlayOnMusicVolumeChange()
    {
        PlayRandomClip(musicSource, melodies);
    }

    public void PlayOnVoiceVolumeChange()
    {
        PlayRandomClip(voiceSource, voiceClips);
    }
}
