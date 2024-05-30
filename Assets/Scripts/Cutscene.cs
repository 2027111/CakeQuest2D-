using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu]
public class Cutscene : BoolValue
{
    public bool repeats = false;
    [JsonIgnore] public Dialogue[] dialogue;
    public int dialogueIndex = 0;
    [JsonIgnore] public RoomInfo StartRoom;
    public override void ApplyData(SavableObject tempCopy)
    {

        repeats = (tempCopy as Cutscene).repeats;
        dialogueIndex = (tempCopy as Cutscene).dialogueIndex;
        base.ApplyData(tempCopy);
    }
    public virtual Dialogue GetLine()
    {

        Dialogue returnValue = GetCurrentLine();
        dialogueIndex++;
        return returnValue;
    }


    public virtual Dialogue GetCurrentLine()
    {
        if (dialogueIndex >= dialogue.Length)
        {
            return null;
        }



        Dialogue returnValue = new Dialogue(dialogue[dialogueIndex]);
        if (returnValue.isNull())
        {
            return null;
        }
        return returnValue;
    }


    public virtual void ResetPlayed()
    {
        dialogueIndex = 0;
    }
}
