using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;
public class MoveSetInfos
{
    public MoveData moveData;
    public CharacterData characterData;

    public MoveSetInfos(MoveData data, CharacterData characterObject)
    {
        this.moveData = data;
        this.characterData = characterObject;
    }
}
public class MoveSetIndicator : MonoBehaviour
{


    [SerializeField] Image CharacterPortraitImage;
    [SerializeField] TMP_Text MoveNameText;

    public MoveSetInfos moveSetInfos;
    private void Start()
    {
        Destroy(gameObject, 3f);
    }
    public void SetPortrait(Sprite sprite)
    {
        if (sprite)
        {
            CharacterPortraitImage.enabled = true;
            CharacterPortraitImage.sprite = sprite;
        }
        else
        {
            CharacterPortraitImage.enabled = false;
        }
    }
    public void SetMoveSetInfos(MoveSetInfos msi)
    {
        moveSetInfos = msi;

        SetPortrait(moveSetInfos.characterData.GetPortrait());
        SetAttackName(moveSetInfos.moveData.MoveName);
    }



    public void SetAttackName(string text)
    {
        MoveNameText.text = text;
    }
}
