using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu]
public class Cutscene : BoolValue
{
    public bool repeats = false;
    public Dialogue[] dialogue;
    public int dialogueIndex = 0;
    public RoomInfo StartRoom;

    public Dialogue GetLine()
    {
        
        Dialogue returnValue = GetCurrentLine();
        dialogueIndex++;
        return returnValue;
    }


    public Dialogue GetCurrentLine()
    {
        if (dialogueIndex >= dialogue.Length)
        {
            return null;
        }
        Dialogue returnValue = new Dialogue(dialogue[dialogueIndex]);
        return returnValue;
    }
}
