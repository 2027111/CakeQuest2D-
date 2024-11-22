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
    
    public Dialogue[] dialogue;
    
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
        dialogueIndex++;
        return returnValue;
    }


    public virtual void ForceRuntime()
    {
        RuntimeValue = true;
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
