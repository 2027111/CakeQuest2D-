using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Quest", menuName = "Quests/QuestObject")]
public class QuestObject : BoolValue
{
    public string questId;
    public string questName;
    public bool QuestToggled = false;
    [JsonIgnore]public QuestObject NextQuestObject;
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
        UICanvas.UpdateQuestList();
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
        string returnValue = "";
        if (RuntimeValue)
        {
            returnValue = "done";
            string newDesc = LanguageData.GetDataById("Indications").GetValueByKey("done");
            if (newDesc != "E404")
            {
                returnValue = newDesc;
            }
        }
        return returnValue;
    }

    public void ToggleQuest(bool on = true)
    {
        QuestToggled = on;
        if (on)
        {
            AddCheckEvent();
        }
        else
        {

                if (NextQuestObject != null)
                {
                    QuestManager.Singleton?.GiveQuest(NextQuestObject);
                }
           
            RemoveCheckEvent();
        }
    }

    public virtual void AddCheckEvent()
    {
        
    }


    public virtual void RemoveCheckEvent()
    {

    }

}
