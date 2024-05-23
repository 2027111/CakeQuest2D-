using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwapButton : ChoiceMenuButton
{

    [SerializeField] Image characterImage;


    public void SetCharacter(BattleCharacter battleCharacter)
    {
        CharacterData data = battleCharacter.GetData();
        characterImage.sprite = data.GetPortrait();
    }
}
