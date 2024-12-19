using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Cutscene), true)]
public class DSCutsceneInspector : Editor
{
   
    protected SerializedProperty dialogueContainerProperty;
    protected SerializedProperty dialogueGroupProperty;
    protected SerializedProperty dialogueProperty;

    protected SerializedProperty groupedDialoguesProperty;
    protected SerializedProperty startingDialoguesOnlyProperty;

    protected SerializedProperty selectedDialogueGroupIndexProperty;
    protected SerializedProperty selectedDialogueIndexProperty;

    protected List<string> dialogueNames;
    protected string dialogueFolderPath;
    protected string dialogueInfoMessage;



    private SerializedProperty repeatsProperty;
    private SerializedProperty startRoomProperty;
    private SerializedProperty cutsceneToPlayProperty;

    // Inherited from BoolValue
    private SerializedProperty runtimeValueProperty;
    private SerializedProperty idProperty;

    protected void OnEnable()
    {
        dialogueContainerProperty = serializedObject.FindProperty("dialogueContainer");
        dialogueGroupProperty = serializedObject.FindProperty("dialogueGroup");
        dialogueProperty = serializedObject.FindProperty("dialogue");

        groupedDialoguesProperty = serializedObject.FindProperty("groupedDialogues");
        startingDialoguesOnlyProperty = serializedObject.FindProperty("startingDialoguesOnly");

        repeatsProperty = serializedObject.FindProperty("repeats");
        startRoomProperty = serializedObject.FindProperty("StartRoom");
        cutsceneToPlayProperty = serializedObject.FindProperty("CutsceneToPlay");

        runtimeValueProperty = serializedObject.FindProperty("RuntimeValue");
        idProperty = serializedObject.FindProperty("UID");

        selectedDialogueGroupIndexProperty = serializedObject.FindProperty("selectedDialogueGroupIndex");
        selectedDialogueIndexProperty = serializedObject.FindProperty("selectedDialogueIndex");
    }
    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        // Draw inherited fields from BoolValue
        DrawBoolValueFields();

        DrawCutsceneFields();

        DrawDialogueContainerArea();

        DSDialogueContainerSO currentDialogueContainer = (DSDialogueContainerSO)dialogueContainerProperty.objectReferenceValue;

        if (currentDialogueContainer == null)
        {
            StopDrawing("Select a Dialogue Container to see the rest of the Inspector.");
            return;
        }

        DrawFiltersArea();

        bool currentGroupedDialoguesFilter = groupedDialoguesProperty.boolValue;
        bool currentStartingDialoguesOnlyFilter = startingDialoguesOnlyProperty.boolValue;

        dialogueFolderPath = $"Assets/DialogueSystem/Dialogues/{currentDialogueContainer.FileName}";


        if (currentGroupedDialoguesFilter)
        {
            List<string> dialogueGroupNames = currentDialogueContainer.GetDialogueGroupNames();
            if (dialogueGroupNames.Count == 0)
            {
                StopDrawing("There are no Dialogue Groups in this Dialogue Container.");
                return;
            }

            DrawDialogueGroupArea(currentDialogueContainer, dialogueGroupNames);

            DSDialogueGroupSO dialogueGroup = (DSDialogueGroupSO)dialogueGroupProperty.objectReferenceValue;
            dialogueNames = currentDialogueContainer.GetGroupedDialogueNames(dialogueGroup, currentStartingDialoguesOnlyFilter);

            dialogueFolderPath += $"/Groups/{dialogueGroup.GroupName}/Dialogues";
            dialogueInfoMessage = "There are no" + (currentStartingDialoguesOnlyFilter ? " Starting" : "") + " Dialogues in this Dialogue Group.";
        }
        else
        {
            dialogueNames = currentDialogueContainer.GetUngroupedDialogueNames(currentStartingDialoguesOnlyFilter);

            dialogueFolderPath += "/Global/Dialogues";
            dialogueInfoMessage = "There are no" + (currentStartingDialoguesOnlyFilter ? " Starting" : "") + " Ungrouped Dialogues in this Dialogue Container.";
        }

        if (dialogueNames.Count == 0)
        {
            StopDrawing(dialogueInfoMessage);
            return;
        }

        DrawDialogueArea(dialogueNames, dialogueFolderPath);

        DrawDialogueEventsArea(); // Add this call to render DialogueEvents

        serializedObject.ApplyModifiedProperties();
    }
    private void DrawBoolValueFields()
    {
        DSInspectorUtility.DrawHeader("Bool Value Properties");
        runtimeValueProperty.DrawPropertyField();
        idProperty.DrawPropertyField();
        DSInspectorUtility.DrawSpace();
    }

    private void DrawCutsceneFields()
    {
        DSInspectorUtility.DrawHeader("Cutscene Configuration");
        repeatsProperty.DrawPropertyField();
        startRoomProperty.DrawPropertyField();
        cutsceneToPlayProperty.DrawPropertyField();
        DSInspectorUtility.DrawSpace();
    }
    private void DrawDialogueEventsArea()
    {
        DSInspectorUtility.DrawHeader("Dialogue Events");

        SerializedProperty dialogueEventsProperty = serializedObject.FindProperty("DialogueEvents");

        if (dialogueEventsProperty != null)
        {
            EditorGUILayout.PropertyField(dialogueEventsProperty, true); // Draw the DialogueEvents array
        }

        DSInspectorUtility.DrawSpace();
    }


    private void StopDrawing(string reason, MessageType messageType = MessageType.Warning)
    {
        DSInspectorUtility.DrawHelpBox(reason, messageType);

        DSInspectorUtility.DrawSpace();

        DSInspectorUtility.DrawHelpBox("You need to select a Dialogue for this component to work properly at Runtime!", MessageType.Warning);

        serializedObject.ApplyModifiedProperties();
    }

    private void DrawDialogueContainerArea()
    {
        DSInspectorUtility.DrawHeader("Dialogue Container");
        dialogueContainerProperty.DrawPropertyField();
        DSInspectorUtility.DrawSpace();
    }
    private void DrawFiltersArea()
    {
        DSInspectorUtility.DrawHeader("Filters");
        groupedDialoguesProperty.DrawPropertyField();
        startingDialoguesOnlyProperty.DrawPropertyField();
        DSInspectorUtility.DrawSpace();
    }
    private void DrawDialogueGroupArea(DSDialogueContainerSO dialogueContainer, List<string> dialogueGroupNames)
    {
        DSInspectorUtility.DrawHeader("Dialogue Group");

        int oldSelectedDialogueGroupIndex = selectedDialogueGroupIndexProperty.intValue;

        DSDialogueGroupSO oldDialogueGroup = (DSDialogueGroupSO)dialogueGroupProperty.objectReferenceValue;

        bool isOldDialogueGroupNull = oldDialogueGroup == null;

        string oldDialogueGroupName = isOldDialogueGroupNull ? "" : oldDialogueGroup.GroupName;

        UpdateIndexOnNamesListUpdate(dialogueGroupNames, selectedDialogueGroupIndexProperty, oldSelectedDialogueGroupIndex, oldDialogueGroupName, isOldDialogueGroupNull);

        selectedDialogueGroupIndexProperty.intValue = DSInspectorUtility.DrawPopup("Dialogue Group", selectedDialogueGroupIndexProperty, dialogueGroupNames.ToArray());

        string selectedDialogueGroupName = dialogueGroupNames[selectedDialogueGroupIndexProperty.intValue];

        DSDialogueGroupSO selectedDialogueGroup = DSIOUtility.LoadAsset<DSDialogueGroupSO>($"Assets/DialogueSystem/Dialogues/{dialogueContainer.FileName}/Groups/{selectedDialogueGroupName}", selectedDialogueGroupName);

        dialogueGroupProperty.objectReferenceValue = selectedDialogueGroup;

        DSInspectorUtility.DrawDisabledFields(() => dialogueGroupProperty.DrawPropertyField());

        DSInspectorUtility.DrawSpace();
    }

    protected void UpdateIndexOnNamesListUpdate(List<string> optionNames, SerializedProperty indexProperty, int oldSelectedPropertyIndex, string oldPropertyName, bool isOldPropertyNull)
    {
        if (isOldPropertyNull)
        {
            indexProperty.intValue = 0;

            return;
        }

        bool oldIndexIsOutOfBoundsOfNamesListCount = oldSelectedPropertyIndex > optionNames.Count - 1;
        bool oldNameIsDifferentThanSelectedName = oldIndexIsOutOfBoundsOfNamesListCount || oldPropertyName != optionNames[oldSelectedPropertyIndex];

        if (oldNameIsDifferentThanSelectedName)
        {
            if (optionNames.Contains(oldPropertyName))
            {
                indexProperty.intValue = optionNames.IndexOf(oldPropertyName);

                return;
            }

            indexProperty.intValue = 0;
        }
    }

    private void DrawDialogueArea(List<string> dialogueNames, string dialogueFolderPath)
    {
        DSInspectorUtility.DrawHeader("Dialogue");

        int oldSelectedDialogueIndex = selectedDialogueIndexProperty.intValue;

        DSDialogueSO oldDialogue = (DSDialogueSO)dialogueProperty.objectReferenceValue;

        bool isOldDialogueNull = oldDialogue == null;

        string oldDialogueName = isOldDialogueNull ? "" : oldDialogue.DialogueName;

        UpdateIndexOnNamesListUpdate(dialogueNames, selectedDialogueIndexProperty, oldSelectedDialogueIndex, oldDialogueName, isOldDialogueNull);

        selectedDialogueIndexProperty.intValue = DSInspectorUtility.DrawPopup("Dialogue", selectedDialogueIndexProperty, dialogueNames.ToArray());

        string selectedDialogueName = dialogueNames[selectedDialogueIndexProperty.intValue];

        DSDialogueSO selectedDialogue = DSIOUtility.LoadAsset<DSDialogueSO>(dialogueFolderPath, selectedDialogueName);

        dialogueProperty.objectReferenceValue = selectedDialogue;

        DSInspectorUtility.DrawDisabledFields(() => dialogueProperty.DrawPropertyField());
    }
}
