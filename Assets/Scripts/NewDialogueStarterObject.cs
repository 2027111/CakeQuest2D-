using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;
using System.Reflection;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using System;


[Serializable]
public class Dialogue
{



    public BoolValue condition;
    public string[] dialogueLineIds;
    public ChoiceDialogue[] choices;
    public UnityEvent OnOverEvent;
    public UnityEvent OnInstantOverEvent;
    public GameObject source;

    public virtual void SetPlayed()
    {
        Debug.Log("OK");
    }

    public void SetSource(GameObject source)
    {
        this.source = source;
    }
    public Dialogue(Dialogue dialogue)
    {

        if(dialogue != null)
        {
            if (dialogue.dialogueLineIds != null)
            {
                this.dialogueLineIds = dialogue.dialogueLineIds.Length > 0 ? dialogue.dialogueLineIds : null;
            }
            if (dialogue.choices != null)
            {
                this.choices = dialogue.choices.Length>0?dialogue.choices:null;
            }
            this.condition = dialogue.condition;
            this.OnOverEvent = dialogue.OnOverEvent;
            this.OnInstantOverEvent = dialogue.OnInstantOverEvent;
            this.source = dialogue.source;
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
        if (condition)
        {
            return !condition.RuntimeValue;
        }
        return true;
    }
    public bool isNull()
    {  if(dialogueLineIds == null)
        {
            return true;
        }else if (dialogueLineIds.Length == 0)
        {
            return true;
        }
        else
        {
            foreach(string l in dialogueLineIds)
            {
                if (string.IsNullOrEmpty(l))
                {
                    return true;
                }
            }
        }
        return false;
    }



}

[Serializable]
public class ChoiceDialogue
{

    public BoolValue condition;
    public string choicesLineIds;
    public string[] dialogueLineIds;
    public ChoiceDialogue[] choices;
    public UnityEvent OnOverEvent;
    public UnityEvent OnInstantOverEvent;


}


public enum BattleCondition
{
    None,
    OnLoop,
    OnTurn,
    OnEnemyTurn
}

[Serializable]
public class BattleDialogue : Dialogue
{
    public BattleCondition technicalCondition;
    public int conditionIndex;
    public bool played = false;

    public bool CheckBattleCondition()
    {

        if (played)
        {
            return false;
        }
        switch (technicalCondition)
        {
            case BattleCondition.None:
                return true;
                break;
            case BattleCondition.OnLoop:
                if(BattleManager.Singleton.GetLoopAmount() == conditionIndex && BattleManager.Singleton.IsFirstTurn())
                {
                    return true;
                }
                break;
            case BattleCondition.OnTurn:
                if (BattleManager.Singleton.GetTurnAmount() == conditionIndex && !BattleManager.Singleton.IsEnemyTurn())
                {
                    return true;
                }
                break;
            case BattleCondition.OnEnemyTurn:
                if (BattleManager.Singleton.GetEnemyTurnAmount() == conditionIndex && BattleManager.Singleton.IsEnemyTurn())
                {
                    return true;
                }
                break;
            default:
                break;
        }
        return false;
    }
    public override void SetPlayed()
    {
        played = true;
    }
    public BattleDialogue(Dialogue dialogue) : base(dialogue)
    {
    }
}




public class NewDialogueStarterObject : MonoBehaviour
{
    public Dialogue dialogue;
    public bool started = false;
    public Character player;
    const string fieldNamePattern = "{.*?}";


    public UnityEvent OnDialogueOverEvent;




    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Character>();

        
    }


    public virtual void DialogueAction()
    {
        if (dialogue.dialogueLineIds.Length > 0)
        {
            if (!started)
        {
            started = true;
                DialogueRequest();
            }
        }
        else
        {
            OnDialogueOverEvent.Invoke();
        }
    }
    public virtual void DialogueRequest()
    {
        if (CheckLines())
        {
            Dialogue newDialogue = new Dialogue(dialogue);
            DialogueBox.Singleton.StartDialogue(newDialogue, player.gameObject, gameObject);
        }
        else
        {
            DialogueOver();
        }
    }

    public bool CheckLines()
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

    public virtual void DialogueOver()
    {
        started = false;
        player.ChangeState(new PlayerControlsBehaviour());
        OnDialogueOverEvent?.Invoke();
    }



    //public static LineInfo[] GetFormattedLines<T>(T currentObject, LineInfo[] lines)
    //{
    //    List<LineInfo> formattedLines = new List<LineInfo>();
    //    string fieldNamePattern = "{(.*?)}";

    //    foreach (LineInfo lineInfo in lines)
    //    {
    //        string result = lineInfo.line;

    //        foreach (Match match in Regex.Matches(lineInfo.line, fieldNamePattern, RegexOptions.IgnoreCase))
    //        {
    //            string[] fieldNames = match.Groups[1].Value.Split('.');

    //            // Start with the current object
    //            object fieldValue = currentObject;

    //            foreach (string fieldName in fieldNames)
    //            {
    //                FieldInfo fieldInfo = fieldValue.GetType().GetField(fieldName);

    //                if (fieldInfo != null)
    //                {
    //                    fieldValue = fieldInfo.GetValue(fieldValue);
    //                }
    //                else
    //                {
    //                    Debug.LogWarning($"Field {fieldName} not found in {fieldValue}'s type");
    //                    fieldValue = null;
    //                    break;
    //                }
    //            }

    //            if (fieldValue != null)
    //            {
    //                result = Regex.Replace(result, match.Value, fieldValue.ToString());
    //            }
    //            else
    //            {
    //                // Handle the case when a field is not found or is null
    //                Debug.LogWarning($"Field value not found for placeholder: {match.Value}");
    //            }
    //        }

    //        formattedLines.Add(new LineInfo(lineInfo.portraitPath, lineInfo.talkername, result));
    //    }

    //    return formattedLines.ToArray();
    //}


    public static string GetFormattedLines<T>(T currentObject, string lineInfo)
    {
        string fieldNamePattern = "{(.*?)}";

            string result = lineInfo;

            foreach (Match match in Regex.Matches(lineInfo, fieldNamePattern, RegexOptions.IgnoreCase))
            {
                string[] fieldNames = match.Groups[1].Value.Split('.');

                // Start with the current object
                object fieldValue = currentObject;

                foreach (string fieldName in fieldNames)
                {
                    FieldInfo fieldInfo = fieldValue.GetType().GetField(fieldName);

                    if (fieldInfo != null)
                    {
                        fieldValue = fieldInfo.GetValue(fieldValue);
                    }
                    else
                    {
                        Debug.LogWarning($"Field {fieldName} not found in {fieldValue}'s type");
                        fieldValue = null;
                        break;
                    }
                }

                if (fieldValue != null)
                {
                    result = Regex.Replace(result, match.Value, fieldValue.ToString());
                }
                else
                {
                    // Handle the case when a field is not found or is null
                    Debug.LogWarning($"Field value not found for placeholder: {match.Value}");
                }
            

        }

        return result;
    }

}
