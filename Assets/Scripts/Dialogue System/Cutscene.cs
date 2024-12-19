using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Timeline;

[CreateAssetMenu]
public class Cutscene : BoolValue
{
    public bool repeats = false;

    protected DSDialogueSO currentDialogueSO;



    [SerializeField] protected DSDialogueContainerSO dialogueContainer;
    [SerializeField] protected DSDialogueGroupSO dialogueGroup;
    [SerializeField] protected DSDialogueSO dialogue;

    [SerializeField] private bool groupedDialogues;
    [SerializeField] private bool startingDialoguesOnly;


    [SerializeField] private int selectedDialogueGroupIndex = 0;
    [SerializeField] private int selectedDialogueIndex = 0;

    public RoomInfo StartRoom;
    
    public TimelineAsset CutsceneToPlay;


    public override string GetJsonData()
    {

        var jsonObject = JObject.Parse(base.GetJsonData()); // Start with base class data


        jsonObject["repeats"] = repeats; // Adding additional data

        return jsonObject.ToString();



    }

    public void PlayedCurrentLine()
    {
        Debug.Log("Played current line " + currentDialogueSO.name);
        if(currentDialogueSO.BattleConditionParams != null)
        {
            currentDialogueSO.BattleConditionParams[0].played = true;
        }
    }


    public virtual Dialogue GetNextLine()
    {

        Dialogue returnValue = GetCurrentLine();
        currentDialogueSO = currentDialogueSO.Choices[0].NextDialogue;
        return returnValue;
    }


    public virtual void ForceRuntime()
    {
        RuntimeValue = true;
    }

    public virtual Dialogue GetCurrentLine()
    {



        Dialogue returnValue = new Dialogue(currentDialogueSO);
        if (returnValue.isNull())
        {
            return null;
        }
        return returnValue;
    }


    public virtual void ResetPlayed()
    {
            currentDialogueSO = dialogue;
    }
}

