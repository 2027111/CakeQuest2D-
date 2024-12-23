using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
    public QuestObject NextQuestObject;



    [SerializeField] protected DSDialogueContainerSO dialogueContainer;
    [SerializeField] protected DSDialogueGroupSO dialogueGroup;
    [SerializeField] protected DSDialogueSO dialogue;

    [SerializeField] private bool groupedDialogues;
    [SerializeField] private bool startingDialoguesOnly;


    [SerializeField] private int selectedDialogueGroupIndex = 0;
    [SerializeField] private int selectedDialogueIndex = 0;

    public override string GetJsonData()
    {
        var jsonObject = JObject.Parse(base.GetJsonData()); // Start with base class data
        // Include all non-ignored properties
        jsonObject["QuestToggled"] = QuestToggled;
        return jsonObject.ToString();
    }


    public virtual void CheckConditions()
    {

    }


    public virtual void ShowQuest()
    {

    }
    public virtual void DialogueRequest()
    {
        Dialogue newDialogue = new Dialogue(dialogue);
        newDialogue.OnOverEvent.AddListener(DialogueOver);
        UICanvas.StartDialogueDelayed(newDialogue, Character.Player.gameObject);
    }
    public virtual void DialogueOver()
    {
        Character.Player.ChangeState(new PlayerControlsBehaviour());
    }

    public bool isCompleted()
    {
        return RuntimeValue;
    }

    public virtual void CompleteQuest()
    {


        SetRuntime();
        UICanvas.UpdateQuestList();
        if (CheckLines())
        {
            DialogueRequest();
        }
    }


    public bool CheckLines()
    {
        if (dialogue != null)
        {

            if (dialogue.isNull())
            {
                return false;
            }
            else
            {
                return dialogue.ConditionRespected();
            }
        }


        return false;
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
            newDesc = BranchingDialogueStarterObject.GetFormattedLines(this, newDesc);
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
            string newDesc = LanguageData.GetDataById(LanguageData.INDICATION).GetValueByKey("done");
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

            if (RuntimeValue)
            {
                if (NextQuestObject != null)
                {
                    QuestManager.Singleton?.GiveQuest(NextQuestObject);
                }

                RemoveCheckEvent();
            }

        }
    }

    public virtual void AddCheckEvent()
    {

    }


    public virtual void RemoveCheckEvent()
    {

    }

}
