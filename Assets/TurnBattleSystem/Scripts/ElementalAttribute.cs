using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum Element
{
    Slash,
    Bash,
    Sweet,
    Sour,
    Bitter,
    Salty,
    Support,
}

public enum ElementEffect
{
    Neutral,
    Weak,
    Resistant,
    NonAffected
}


[System.Serializable]
public class ElementalAttribute
{
    public Element element;
    public ElementEffect elementEffect;
}
