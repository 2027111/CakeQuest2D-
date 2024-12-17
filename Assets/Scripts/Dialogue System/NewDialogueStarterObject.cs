using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;
using System.Reflection;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using System;





public class NewDialogueStarterObject : MonoBehaviour
{
    public Dialogue dialogue;
    public bool started = false;


    public UnityEvent OnDialogueOverEvent;




    public void PlayVideo()
    {
        UICanvas.PlayVideoRec();
    }

    public virtual void DialogueAction()
    {
        DialogueRequest();
    }
    public virtual void DialogueRequest()
    {
        if (CheckLines())
        {
            if (!started)
            {
                started = true;
                Dialogue newDialogue = new Dialogue(dialogue);
                UICanvas.StartDialogue(newDialogue, Character.Player.gameObject, gameObject);
            }
        }
        else
        {
            DialogueOver();
        }
    }

    public bool CheckLines()
    {
        Debug.Log("Checking Lines");
        if (dialogue.isNull())
        {
            Debug.Log("Dialogue is NUll?");
            return false;
        }
        else
        {
            Debug.Log("Dialogue is not NUll");
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


    public static string GetFormattedLines(object currentObject, string lineInfo)
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
                result = result.Replace(match.Value, fieldValue.ToString());
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
