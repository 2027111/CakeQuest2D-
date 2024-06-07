using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestObject : BoolValue
{
    public string questId;
    public string questName;
    public bool QuestToggled = false;
    public virtual void CheckConditions()
    {

    }


    public virtual void ShowQuest()
    {

    }


    public bool isCompleted()
    {
        return RuntimeValue;
    }

    public virtual void CompleteQuest()
    {
        SetRuntime();
    }





    public virtual string GetName()
    {
        string name = questName;
        string newName = LanguageData.GetDataById("quest_" + questId).GetValueByKey("questName");
        if (newName != "E404")
        {
            return newName;
        }
        return name;
    }

    public virtual string GetDescription()
    {
        string desc = "";
        string newDesc = LanguageData.GetDataById("quest_" + questId).GetValueByKey("questDescription");
        if (newDesc != "E404")
        {
            newDesc = NewDialogueStarterObject.GetFormattedLines(this, newDesc);
            return newDesc;
        }
        return desc;
    }
    public virtual string GetObjectiveProgress()
    {
        string desc = "Completed";
        return desc;
    }

    public void ToggleQuest(bool on = true)
    {
        QuestToggled = on;
    }
}
