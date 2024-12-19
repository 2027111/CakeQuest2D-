using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;
using System.Reflection;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using System;

#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(DialogueEvent))]
public class DialogueEventDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        // Draw foldout
        Rect foldoutRect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
        property.isExpanded = EditorGUI.Foldout(foldoutRect, property.isExpanded, label, true);

        if (property.isExpanded)
        {
            EditorGUI.indentLevel++;

            // Dropdown for indexValue
            SerializedProperty indexValueProperty = property.FindPropertyRelative("indexValue");
            List<string> options = DialogueEvent.IndexOptions;

            // Get the current index of the string value in the options list
            int currentIndex = options.IndexOf(indexValueProperty.stringValue);
            if (currentIndex == -1) currentIndex = 0; // Default to "None" if not found

            // Dropdown rect for indexValue
            Rect dropdownRect = new Rect(position.x, position.y + EditorGUIUtility.singleLineHeight + 2, position.width, EditorGUIUtility.singleLineHeight);
            int selectedIndex = EditorGUI.Popup(dropdownRect, "Index Value", currentIndex, options.ToArray());
            indexValueProperty.stringValue = options[selectedIndex];

            // Dropdown for EventType (enum)
            SerializedProperty eventTypeProperty = property.FindPropertyRelative("EventType");
            Rect enumRect = new Rect(position.x, dropdownRect.y + EditorGUIUtility.singleLineHeight + 2, position.width, EditorGUIUtility.singleLineHeight);
            EditorGUI.PropertyField(enumRect, eventTypeProperty);

            // UnityEvent field
            SerializedProperty eventProperty = property.FindPropertyRelative("eventAction");
            Rect eventRect = new Rect(position.x, enumRect.y + EditorGUIUtility.singleLineHeight + 4, position.width, EditorGUI.GetPropertyHeight(eventProperty));
            EditorGUI.PropertyField(eventRect, eventProperty);

            EditorGUI.indentLevel--;
        }

        EditorGUI.EndProperty();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        float height = EditorGUIUtility.singleLineHeight; // Foldout line height

        if (property.isExpanded)
        {
            height += EditorGUIUtility.singleLineHeight + 2; // Dropdown height (indexValue)
            height += EditorGUIUtility.singleLineHeight + 2; // Dropdown height (EventType)
            height += EditorGUI.GetPropertyHeight(property.FindPropertyRelative("eventAction")) + 4; // UnityEvent height
        }

        return height;
    }
}
#endif

[Serializable]
public class DialogueEvent
{
    [SerializeField] private string indexValue = "None"; // Dropdown index value
    [SerializeField] private UnityEvent eventAction; // UnityEvent
    [SerializeField] public DialogueEventType EventType = DialogueEventType.OnOver; // Enum for event type

    // Property for the indexValue
    public string IndexValue
    {
        get => indexValue;
        set => indexValue = value;
    }
    public UnityEvent EventAction => eventAction; // Expose UnityEvent

    // Custom inspector for the dropdown
    public static List<string> IndexOptions = new List<string> { "None" };

    static DialogueEvent()
    {
        for (int i = 0; i <= 15; i++)
        {
            IndexOptions.Add(i.ToString());
        }
    }
}

// Enum for Dialogue Event Types
public enum DialogueEventType
{
    Instant,
    OnOver
}


public class BranchingDialogueStarterObject : MonoBehaviour
{
    public DialogueEvent[] DialogueEvents;
    public bool started = false;



    [SerializeField] private DSDialogueContainerSO dialogueContainer;
    [SerializeField] private DSDialogueGroupSO dialogueGroup;
    [SerializeField] protected DSDialogueSO dialogue;

    [SerializeField] private bool groupedDialogues;
    [SerializeField] private bool startingDialoguesOnly;


    [SerializeField] private int selectedDialogueGroupIndex = 0;
    [SerializeField] private int selectedDialogueIndex = 0;


    private void Start()
    {
        started = false;
    }
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
                Dialogue newDialogue = new Dialogue(dialogue, DialogueEvents, new UnityAction(DialogueOver));
                Debug.Log(newDialogue.DialogueEvents.Length);
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

    }



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
