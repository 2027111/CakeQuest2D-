using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeHandler : MonoBehaviour
{
    [SerializeField] AudioMixer contextMixer;
    [SerializeField] Slider contextSlider;
    [SerializeField] bool SFX;
    [SerializeField] bool Music;
    [SerializeField] bool Voice;


    private void Start()
    {
        if (SFX)
        {
            contextSlider.SetValueWithoutNotify(GamePreference.SFXVolume);
            SetSFXPreference(GamePreference.SFXVolume);
        }else if (Music)
        {
            contextSlider.SetValueWithoutNotify(GamePreference.MusicVolume);
            SetMusicPreference(GamePreference.MusicVolume);

        }
        else if (Voice)
        {
            contextSlider.SetValueWithoutNotify(GamePreference.VoiceVolume);
            SetVoicePreference(GamePreference.VoiceVolume);

        }
    }
    public void SetVolume (float volume)
    {
        contextMixer.SetFloat("Volume", volume);
        if (SFX)
        {
            SetSFXPreference(volume);
        }
        else if (Music)
        {
            SetMusicPreference(volume);

        }
        else if (Voice)
        {
            SetVoicePreference(volume);

        }
    }



    public void SetSFXPreference(float volume)
    {
        GamePreference.SFXVolume = volume; 
    }
    public void SetMusicPreference(float volume)
    {
        GamePreference.MusicVolume = volume;

    }
    public void SetVoicePreference(float volume)
    {
        GamePreference.VoiceVolume = volume;

    }
}
