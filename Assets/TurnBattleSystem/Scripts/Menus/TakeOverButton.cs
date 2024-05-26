using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class TakeOverButton : ChoiceMenuButton
{
    [SerializeField] Image image;
    BattleCharacter battleCharacter;
    [SerializeField] AudioClip appearAudioClip;
    [SerializeField] AudioClip confirmAudioClip;
    [SerializeField] AudioClip disappearAudioClip;
    public void SetBattleCharacter(BattleCharacter bc)
    {
        if (bc.GetReference())
        {
            image.sprite = bc.GetData().GetPortrait();
        }
        battleCharacter = bc;
    }
    private void Start()
    {
        battleCharacter.PlaySFX(appearAudioClip);
    }
    public void Confirm()
    {
        AudioClip TakeOverClip = Resources.Load<AudioClip>("VoiceLines/Battle_" + battleCharacter.GetData().characterName + "_TakeOver");
        Debug.Log(TakeOverClip == null);
        battleCharacter.PlayVoiceLine(TakeOverClip);
        battleCharacter.PlaySFX(confirmAudioClip);
    }



    public void Over()
    {
        battleCharacter.PlaySFX(disappearAudioClip);
        OnSelected.RemoveAllListeners();
        Animator anim = GetComponent<Animator>();
        anim?.SetTrigger("Over");
    }

    public void DestroySelf()
    {
        Destroy(gameObject);
    }
}