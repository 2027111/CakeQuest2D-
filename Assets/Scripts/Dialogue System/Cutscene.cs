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
    public int dialogueIndex = 0;

    private DSDialogueSO currentDialogueSO;


    [SerializeField] private DSDialogueContainerSO dialogueContainer;
    [SerializeField] private DSDialogueGroupSO dialogueGroup;
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
        jsonObject["dialogueIndex"] = dialogueIndex; // Adding additional data

        return jsonObject.ToString();



    }





    public virtual Dialogue GetNextLine()
    {

        Dialogue returnValue = GetCurrentLine();
        currentDialogueSO = currentDialogueSO.Choices[0].NextDialogue;
        dialogueIndex++;
        return returnValue;
    }


    public virtual void ForceRuntime()
    {
        RuntimeValue = true;
    }

    public virtual Dialogue GetCurrentLine()
    {
        if (dialogueIndex >= dialogueContainer.DialogueGroups[dialogueGroup].Count)
        {
            return null;
        }



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
           dialogueIndex = 0;
    }
}

