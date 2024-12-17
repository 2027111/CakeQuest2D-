using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

using Unity.VisualScripting;

[Serializable, Inspectable]
public class Dialogue
{



    [Inspectable]public ConditionResultObject[] condition;
    public DialogueEvent[] DialogueEvents { get; private set; }

    [Inspectable] public string[] dialogueLineIds;
    [Inspectable] public ChoiceDialogue[] choices;
    [Inspectable] public DSDialogueChoiceData[] DialogueChoices;
    [Inspectable] public string EventIndex;
    [Inspectable] public UnityEvent OnOverEvent;
    [Inspectable] public UnityEvent OnInstantOverEvent;

    public object source = null;

    public void SetSource(object source)
    {
        this.source = source;
    }

    public virtual void SetPlayed()
    {
    }
    public Dialogue(Dialogue dialogue)
    {

        if (dialogue != null)
        {
            if (dialogue.dialogueLineIds != null)
            {
                this.dialogueLineIds = dialogue.dialogueLineIds.Length > 0 ? dialogue.dialogueLineIds : null;
            }
            if (dialogue.choices != null)
            {
                this.choices = dialogue.choices.Length > 0 ? dialogue.choices : null;
            }
            this.condition = dialogue.condition;
            this.OnOverEvent = dialogue.OnOverEvent;
            this.OnInstantOverEvent = dialogue.OnInstantOverEvent;
            this.source = dialogue.source;
        }
    }
    public Dialogue(DSDialogueSO dialogue, DialogueEvent[] events = null, UnityAction dialogueOverCallback = null)
    {

        if (dialogue != null)
        {
            this.dialogueLineIds = dialogue.Text.ToArray();
            this.DialogueChoices = dialogue.Choices.ToArray();
            this.condition = dialogue.Conditions.ToArray();
            this.EventIndex = dialogue.EventIndex;
            this.DialogueEvents = events;
            this.OnOverEvent = new UnityEvent();
            if(dialogueOverCallback != null)
            {
                this.OnOverEvent.AddListener(dialogueOverCallback);
            }
        }
    }


    public Dialogue(string singleLine)
    {

        if (!string.IsNullOrEmpty(singleLine))
        {
            this.dialogueLineIds = new string[1];
            this.dialogueLineIds[0] = singleLine;
            this.choices = null;
            this.condition = null;
            this.OnOverEvent = new UnityEvent();
            this.OnInstantOverEvent = new UnityEvent();
        }
    }
    public Dialogue(ChoiceDialogue dialogue)
    {
        if (dialogue.dialogueLineIds.Length > 0)
        {
            this.dialogueLineIds = dialogue.dialogueLineIds;
        }
        if (dialogue.choices.Length > 0)
        {
            this.choices = dialogue.choices;
        }
        this.condition = dialogue.condition;
        this.OnOverEvent = dialogue.OnOverEvent;
        this.OnInstantOverEvent = dialogue.OnInstantOverEvent;
    }



    public bool ConditionRespected()
    {
        foreach (ConditionResultObject c in condition)
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
        if (dialogueLineIds == null)
        {
            return true;
        }
        else if (dialogueLineIds.Length == 0 && choices.Length == 0)
        {
            return true;
        }
        else if (dialogueLineIds.Length == 0 && HasOnePossibleChoice())
        {
            return false;
        }
        else
        {
            foreach (string l in dialogueLineIds)
            {
                if (string.IsNullOrEmpty(l))
                {
                    return true;
                }
            }
        }
        return false;
    }

    public ChoiceDialogue[] GetUsableChoices()
    {
        List<ChoiceDialogue> returnChocies = new List<ChoiceDialogue>();
        if (choices == null)
        {
            return null;
        }
        foreach (ChoiceDialogue c in choices)
        {
            if (c.ConditionRespected())
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

    public DSDialogueChoiceData[] GetUsableChoicesList()
    {
        List<DSDialogueChoiceData> returnChocies = new List<DSDialogueChoiceData>();
        if (DialogueChoices == null)
        {
            return null;
        }
        foreach (DSDialogueChoiceData c in DialogueChoices)
        {
            if(c.NextDialogue == null)
            {
                return null;
            }
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

        return GetUsableChoices().Length == 1;
    }
}
