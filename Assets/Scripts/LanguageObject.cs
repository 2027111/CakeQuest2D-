using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum Language
{
    Fran�ais,
    English,
}



[CreateAssetMenu]
public class LanguageObject : ScriptableObject
{
    public Language language = Language.Fran�ais;

    public Language GetLanguage()
    {
        return language;
    }

    public bool SetLanguage(Language value)
    {
        if(language != value)
        {
            language = value;
            return true;
        }
        return false;
    }
}
