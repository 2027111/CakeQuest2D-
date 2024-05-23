using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class VolumeHandler : MonoBehaviour
{
    [SerializeField] AudioMixer contextMixer;
    
    public void SetVolume (float volume)
    {
        contextMixer.SetFloat("Volume", volume);
    }
}
