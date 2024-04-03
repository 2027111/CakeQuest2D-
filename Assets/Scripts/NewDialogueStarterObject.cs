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

    public LineInfo[] dialogueLines;
    public ChoiceDialogue[] choices;
    public UnityEvent OnOverEvent;

    public Dialogue(Dialogue dialogue)
    {
        this.dialogueLines = dialogue.dialogueLines;
        this.choices = dialogue.choices;
        this.OnOverEvent = dialogue.OnOverEvent;
    }


    public Dialogue(ChoiceDialogue dialogue)
    {
        this.dialogueLines = dialogue.dialogueLines;
        this.choices = dialogue.choices;
        this.OnOverEvent = dialogue.OnOverEvent;
    }
}

[Serializable]
public class ChoiceDialogue
{
    public string choicesLine;
    public LineInfo[] dialogueLines;
    public ChoiceDialogue[] choices;
    public UnityEvent OnOverEvent;
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
        if (dialogue.dialogueLines.Length > 0)
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
            DialogueBox.Singleton.StartDialogue(dialogue, player.gameObject, gameObject);
        }
        else
        {
            DialogueOver();
        }
    }

    public bool CheckLines()
    {
        foreach (LineInfo line in dialogue.dialogueLines)
        {
            if (line == null)
            {
                return false;
            }
        }
        return true;
    }

    public virtual void DialogueOver()
    {
        started = false;
        player.ChangeState(new PlayerControlsBehaviour());
        OnDialogueOverEvent?.Invoke();
    }


    public void SetDialogueLines(LineInfo[] newLines)
    {
        dialogue.dialogueLines = newLines;
    }

    public void SetNewDialogue(Dialogue newDialogue)
    {
        dialogue = newDialogue;
    }

    public void DebugPrint()
    {
    }
    public static LineInfo[] GetFormattedLines<T>(T currentObject, LineInfo[] lines)
    {
        List<LineInfo> formattedLines = new List<LineInfo>();
        string fieldNamePattern = "{(.*?)}";

        foreach (LineInfo lineInfo in lines)
        {
            string result = lineInfo.line;

            foreach (Match match in Regex.Matches(lineInfo.line, fieldNamePattern, RegexOptions.IgnoreCase))
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

            formattedLines.Add(new LineInfo(lineInfo.portraitPath, lineInfo.talkername, result));
        }

        return formattedLines.ToArray();
    }

}
