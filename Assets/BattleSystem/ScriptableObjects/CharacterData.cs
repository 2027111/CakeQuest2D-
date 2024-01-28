using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;





[CreateAssetMenu]
public class CharacterData : ScriptableObject
{
    public string characterName;
    public Sprite[] portraits;
    public string HealthName;
    public string ManaName;

    public Sprite GetPortrait()
    {
        if(portraits.Length > 0)
        {
            return portraits[0];
        }
        else
        {
            return null;
        }
    }
}
