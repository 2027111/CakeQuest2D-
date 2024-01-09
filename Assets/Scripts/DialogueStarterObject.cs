using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;
using System.Reflection;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class DialogueStarterObject : MonoBehaviour
{
    public LineInfo[] dialogueLines;
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
        if (dialogueLines.Length > 0)
        {
            if (!started)
        {
            started = true;
                DialogueRequest();
            }
        }
    }
    public virtual void DialogueRequest()
    {
        DialogueBox.Singleton.StartDialogue(GetFormattedLines(this, dialogueLines), DialogueOver, player.gameObject, gameObject);
    }
    public virtual void DialogueOver()
    {
        started = false;
        Debug.Log("Dialogue Over");
        player.ChangeState(new PlayerControlsBehaviour());
        OnDialogueOverEvent?.Invoke();
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

            formattedLines.Add(new LineInfo(lineInfo.portrait, lineInfo.talkername, result));
            Debug.Log(result);
        }

        return formattedLines.ToArray();
    }

}
