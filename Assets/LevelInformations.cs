using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelInformations : MonoBehaviour
{
    public AudioClip levelMainTheme;


    public void Start()
    {
            MusicPlayer.Singleton?.PlaySong(levelMainTheme, true);
        
    }
}
