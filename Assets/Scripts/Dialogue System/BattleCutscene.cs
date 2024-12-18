using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu]
public class BattleCutscene : Cutscene
{
    public BattleDialogueHolder[] battleDialogue;

    public override Dialogue GetNextLine()
    {

        Dialogue returnValue = GetPlayableLine();
        dialogueIndex++;
        return returnValue;
    }
    public override void SetRuntime()
    {
        foreach (BattleDialogueHolder d in battleDialogue)
        {
            if (!d.played)
            {
                return;
            }
        }
        RuntimeValue = true;
    }


    public override void ForceRuntime()
    {
        foreach (BattleDialogueHolder d in battleDialogue)
        {
            if (!d.played)
            {
                d.played = true;
            }
        }
        SetRuntime();
    }
    public override void ResetPlayed()
    {
        foreach (BattleDialogueHolder d in battleDialogue)
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

    public void ForceWeakness(string weakness)
    {
        List<int> elementRecipe = new List<int>();

        foreach (char e in weakness)
        {
            if (int.TryParse(e.ToString(), out int ress))
            {
                elementRecipe.Add(ress);
            }
        }






        BattleManager.Singleton?.ForceRecipe(elementRecipe.ToArray());

    }
    public void DisableOptions(string options)
    {
        List<int> DisabledOptions = new List<int>();

        foreach (char e in options)
        {
            if (int.TryParse(e.ToString(), out int ress))
            {
                DisabledOptions.Add(ress);
            }
        }






        BattleManager.Singleton?.DisableOptions(DisabledOptions.ToArray());

    }
    public BattleDialogue GetPlayableLine()
    {
        BattleDialogue battleDialogue = null;
        for (int i = 0; i < this.battleDialogue.Length; i++)
        {
            BattleDialogueHolder battleDialogueHolder = this.battleDialogue[i];
            
            if (battleDialogueHolder.CheckBattleCondition())
            {
                battleDialogue = new BattleDialogue(battleDialogueHolder); ;
                if (i > 0)
                {
                    if (battleDialogueHolder.requiresPrevious)
                    {
                        for (int j = i - 1; j >= 0; j--)
                        {
                            if (!battleDialogueHolder.played)
                            {
                                return null;
                            }
                        }
                    }

                }
            }
        }
        return battleDialogue;
    }
}