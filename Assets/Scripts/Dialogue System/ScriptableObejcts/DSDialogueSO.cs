using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DSDialogueSO : ScriptableObject
{

    [field: SerializeField] public List<ConditionResultObject> Conditions { get; set; }
    [field: SerializeField] public List<BattleCondition> BattleConditionParams { get; set; } // List of conditions

    [field: SerializeField] public string DialogueName { get; set; }
    [field: SerializeField] public string EventIndex { get; set; }
    [field: SerializeField] [field: TextArea()] public List<string> Text { get; set; }
    [field: SerializeField] public List<DSDialogueChoiceData> Choices { get; set; }

    [field: SerializeField] public DSDialogueType DialogueType { get; set; }
    [field: SerializeField] public bool IsStartingDialogue { get; set; }


    public void Initialize(string dialogueName, List<string> text, List<DSDialogueChoiceData> choices, DSDialogueType dialogueType, bool isStartingDialogue, List<ConditionResultObject> conditions, List<BattleCondition> battleConditionParams, string eventIndex)
    {
        DialogueName = dialogueName;
        Text = text;
        Choices = choices;
        DialogueType = dialogueType;
        IsStartingDialogue = isStartingDialogue;
        Conditions = conditions;
        BattleConditionParams = battleConditionParams;
        SetRequirePrevious();
        EventIndex = eventIndex;
    }

    private void SetRequirePrevious()
    {
        if (BattleConditionParams != null)
        {
            BattleConditionParams[0].requiresPrevious = !IsStartingDialogue;
        }
    }

    public bool ConditionRespected()
    {
        foreach (ConditionResultObject c in Conditions)
        {
            if (!c.CheckCondition())
            {
                return false;
            }
        }
        return true;
    }
    public bool isNull()
    {
        if (Text == null)
        {
            return true;
        }
        else if (Text.Count == 0 && Choices.Count == 0)
        {
            return true;
        }
        else if (Text.Count == 0 && HasOnePossibleChoice())
        {
            return false;
        }
        return false;
    }

    public DSDialogueChoiceData[] GetUsableChoices()
    {
        List<DSDialogueChoiceData> returnChocies = new List<DSDialogueChoiceData>();
        if (Choices == null)
        {
            return null;
        }
        foreach (DSDialogueChoiceData c in Choices)
        {
            if (c.NextDialogue.ConditionRespected())
            {
                returnChocies.Add(c);
            }
        }
        if (returnChocies.Count == 0)
        {
            return null;
        }
        return returnChocies.ToArray();
    }

    public bool HasOnePossibleChoice()
    {
        return GetUsableChoices()?.Length == 1;
    }
}
