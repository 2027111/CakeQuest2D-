using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu]
public class BattleCutscene : Cutscene
{

    public DialogueEvent[] DialogueEvents;
    public override Dialogue GetNextLine()
    {
        currentDialogueSO = GetPlayableLine();
        Dialogue returnValue = new Dialogue(currentDialogueSO, DialogueEvents); 
        return returnValue;
    }
    public override void SetRuntime()
    {
        foreach (DSDialogueSO d in dialogueContainer.DialogueGroups[dialogueGroup])
        {
            if (d.BattleConditionParams[0].played != true)
            {
                return;
            }
        }
        RuntimeValue = true;
    }


    public override void ForceRuntime()
    {
        foreach (DSDialogueSO d in dialogueContainer.DialogueGroups[dialogueGroup])
        {
            if(d.BattleConditionParams != null)
            {
                d.BattleConditionParams[0].played = true;
            }
        }
        SetRuntime();
    }
    public override void ResetPlayed()
    {
        base.ResetPlayed();

        foreach (DSDialogueSO d in dialogueContainer.DialogueGroups[dialogueGroup])
        {
            if (d.BattleConditionParams != null)
            {
                d.BattleConditionParams[0].played = false;
                d.BattleConditionParams[0].requiresPrevious = !d.IsStartingDialogue;
            }
        }
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
    public DSDialogueSO GetPlayableLine()
    {
        for (int i = 0; i < this.dialogueContainer.DialogueGroups[dialogueGroup].Count; i++)
        {
            DSDialogueSO battleDialogueHolder = this.dialogueContainer.DialogueGroups[dialogueGroup][i];
            if(battleDialogueHolder.BattleConditionParams == null)
            {
                continue;
            }
            if (battleDialogueHolder.BattleConditionParams[0].CheckBattleCondition())
            {
                if (i > 0)
                {
                    if (battleDialogueHolder.BattleConditionParams[0].requiresPrevious)
                    {
                                return null;
                    }

                }

                return battleDialogueHolder;
            }
        }
        return null;
    }
}