using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelInformations : MonoBehaviour
{
    public AudioClip levelMainTheme;
    public Color CameraBackgroundColor;

    public void Start()
    {
            MusicPlayer.Singleton?.PlaySong(levelMainTheme, true);
        Camera.main.backgroundColor = CameraBackgroundColor;


    }
}
