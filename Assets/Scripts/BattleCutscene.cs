using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu]
public class BattleCutscene : Cutscene
{
    public BattleDialogue[] battleDialogue;

    public override Dialogue GetLine()
    {

        Dialogue returnValue = GetPlayableLine();
        dialogueIndex++;
        return returnValue;
    }
    public override void SetRuntime()
    {
        foreach (BattleDialogue d in battleDialogue)
        {
            if (!d.played)
            {
                return;
            }
        }
        RuntimeValue = true;
    }

    public override void ResetPlayed()
    {
        foreach (BattleDialogue d in battleDialogue)
        {
            d.played = false;
        }
    }

    public override Dialogue GetCurrentLine()
    {
        if (dialogueIndex >= battleDialogue.Length)
        {
            return null;
        }



        Dialogue returnValue = new BattleDialogue(battleDialogue[dialogueIndex]);
        if (returnValue.isNull())
        {
            return null;
        }
        return returnValue;
    }

    public BattleDialogue GetPlayableLine()
    {
        BattleDialogue b = null;
        foreach (BattleDialogue bd in battleDialogue)
        {

            if (bd.CheckBattleCondition())
            {
                b= bd;
            }
        }
        Debug.Log(b);
        return b;
    }
}