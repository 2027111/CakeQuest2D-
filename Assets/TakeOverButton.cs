using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TakeOverButton : ChoiceMenuButton
{
    [SerializeField] Image image;
    public void SetBattleCharacter(BattleCharacter bc)
    {
        if (bc.GetReference())
        {
            image.sprite = bc.GetData().GetPortrait();
        }
    }
}
