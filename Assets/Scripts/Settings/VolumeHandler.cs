using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;

public class VolumeHandler : MonoBehaviour
{
    [SerializeField] AudioMixer contextMixer;
    [SerializeField] Slider contextSlider;
    [SerializeField] bool SFX;
    [SerializeField] bool Music;
    [SerializeField] bool Voice;
    [SerializeField] TMP_Text volumeText;


    private void Start()
    {
        int volume = 0;
        if (SFX)
        {
            volume = GamePreference.SFXVolume;
            contextSlider.SetValueWithoutNotify(GamePreference.SFXVolume);
            SetSFXPreference(GamePreference.SFXVolume);
        }else if (Music)
        {
            volume = GamePreference.MusicVolume;
            contextSlider.SetValueWithoutNotify(GamePreference.MusicVolume);
            SetMusicPreference(GamePreference.MusicVolume);

        }
        else if (Voice)
        {
            volume = GamePreference.VoiceVolume;
            contextSlider.SetValueWithoutNotify(GamePreference.VoiceVolume);
            SetVoicePreference(GamePreference.VoiceVolume);

        }

        SetVolume(volume);
        
    }
    public void SetVolume (float volume)
    {
        contextMixer.SetFloat("Volume", volume);
        volumeText.SetText(((int)volume + 50).ToString());
        if (SFX)
        {
            SetSFXPreference((int)volume);
        }
        else if (Music)
        {
            SetMusicPreference((int)volume);

        }
        else if (Voice)
        {
            SetVoicePreference((int)volume);

        }
    }



    public void SetSFXPreference(int volume)
    {
        GamePreference.SFXVolume = volume; 
    }
    public void SetMusicPreference(int volume)
    {
        GamePreference.MusicVolume = volume;

    }
    public void SetVoicePreference(int volume)
    {
        GamePreference.VoiceVolume = volume;

    }
}
