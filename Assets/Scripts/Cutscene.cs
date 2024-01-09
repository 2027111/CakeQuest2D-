using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu]
public class Cutscene : BoolValue
{
    public bool repeats = false;
    public DialogueContent[] dialogueContents;
    public int dialogueIndex = 0;

    public LineInfo[] GetLine()
    {
        if(dialogueIndex >= dialogueContents.Length)
        {
            return null;
        }
        LineInfo[] returnValue = dialogueContents[dialogueIndex].lines;
        dialogueIndex++;
        return returnValue;
    }

    public LineInfo[] GetCurrentLine()
    {
        if (dialogueIndex >= dialogueContents.Length)
        {
            return null;
        }
        LineInfo[] returnValue = dialogueContents[dialogueIndex].lines;
        return returnValue;
    }
}
