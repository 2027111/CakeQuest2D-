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



    public ConditionObject[] condition;

    public string[] dialogueLineIds;
    public ChoiceDialogue[] choices;
    public UnityEvent OnOverEvent;
    public UnityEvent OnInstantOverEvent;

    public GameObject source = null;

    public void SetSource(GameObject source)
    {
        this.source = source;
    }

    public virtual void SetPlayed()
    {
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
        foreach (ConditionObject c in condition)
        {
            if (!c.CheckCondition())
            {
                return false;
            }
        }
        return true;
    }
    public bool isNull()
    {  if(dialogueLineIds == null)
        {
            return true;
        }else if (dialogueLineIds.Length == 0 && choices.Length == 0)
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

    public ChoiceDialogue[] GetUsableChoices()
    {
        List<ChoiceDialogue> returnChocies = new List<ChoiceDialogue>();
        if(choices == null)
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
        if(returnChocies.Count == 0)
        {
            return null;
        }
        return returnChocies.ToArray();
    }
}

[Serializable]
public class ChoiceDialogue
{

    public ConditionObject[] condition;
    public string choicesLineIds;
    public string[] dialogueLineIds;
    public ChoiceDialogue[] choices;
    public UnityEvent OnOverEvent;
    public UnityEvent OnInstantOverEvent;
    public bool ConditionRespected()
    {
        foreach (ConditionObject c in condition)
        {
            if (!c.CheckCondition())
            {
                return false;
            }
        }
        return true;
    }

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
    const string fieldNamePattern = "{.*?}";


    public UnityEvent OnDialogueOverEvent;




    public void PlayVideo()
    {
        Debug.Log("Called Play Video");
        UICanvas.PlayVideoRec();
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
            UICanvas.StartDialogue(newDialogue, Character.Player.gameObject, gameObject);
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
    public void CancelDialogue()
    {
        UICanvas.CancelCurrentDialogue();
    }

    public virtual void DialogueOver()
    {
        started = false;
        Character.Player.ChangeState(new PlayerControlsBehaviour());
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
                if (fieldValue == null)
                {
                    break;
                }

                Type fieldType = fieldValue.GetType();

                // Check for a field
                FieldInfo fieldInfo = fieldType.GetField(fieldName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                if (fieldInfo != null)
                {
                    fieldValue = fieldInfo.GetValue(fieldValue);
                    continue;
                }

                // Check for a property
                PropertyInfo propertyInfo = fieldType.GetProperty(fieldName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                if (propertyInfo != null)
                {
                    fieldValue = propertyInfo.GetValue(fieldValue);
                    continue;
                }

                // Check for a method
                MethodInfo methodInfo = fieldType.GetMethod(fieldName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                if (methodInfo != null && methodInfo.GetParameters().Length == 0)
                {
                    fieldValue = methodInfo.Invoke(fieldValue, null);
                    continue;
                }

                // If we get here, the field/property/method was not found
                Debug.LogWarning($"Field, property, or method {fieldName} not found in {fieldType}");
                fieldValue = null;
                break;
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
