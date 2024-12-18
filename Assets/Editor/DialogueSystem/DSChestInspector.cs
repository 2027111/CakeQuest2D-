using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TreasureChest))]
public class ChestDSInspector : DSInspector
{
    private SerializedProperty contentProperty;
    private SerializedProperty amountProperty;
    private SerializedProperty onlyOnceProperty;
    private SerializedProperty isOpenProperty;
    private SerializedProperty lockedProperty;
    private SerializedProperty lockRequirementProperty;
    private SerializedProperty requirementAmountProperty;
    private SerializedProperty consumesRequirementProperty;
    private SerializedProperty lockedDialogueProperty;
    private SerializedProperty successDialogueProperty;
    private SerializedProperty storedOpenProperty;
    private SerializedProperty onIsOpenProperty;



    private SerializedProperty selectedSuccessDialogueIndexProperty;
    private SerializedProperty selectedLockedDialogueIndexProperty;


    protected void OnEnable()
    {
        base.OnEnable();

        // TreasureChest-specific properties
        contentProperty = serializedObject.FindProperty("content");
        amountProperty = serializedObject.FindProperty("amount");
        onlyOnceProperty = serializedObject.FindProperty("OnlyOnce");
        isOpenProperty = serializedObject.FindProperty("isOpen");
        lockedProperty = serializedObject.FindProperty("locked");
        lockRequirementProperty = serializedObject.FindProperty("lockRequirement");
        requirementAmountProperty = serializedObject.FindProperty("requirementAmount");
        consumesRequirementProperty = serializedObject.FindProperty("consumesRequirement");
        lockedDialogueProperty = serializedObject.FindProperty("LockedDialogue");
        successDialogueProperty = serializedObject.FindProperty("SuccessDialogue");
        storedOpenProperty = serializedObject.FindProperty("storedOpen");
        onIsOpenProperty = serializedObject.FindProperty("OnIsOpen");


        selectedSuccessDialogueIndexProperty = serializedObject.FindProperty("selectedSuccessDialogueIndex");
        selectedLockedDialogueIndexProperty = serializedObject.FindProperty("selectedLockedDialogueIndex");

    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        // Draw the base DSInspector fields
        base.OnInspectorGUI();

        // Draw TreasureChest-specific fields
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Treasure Chest Properties", EditorStyles.boldLabel);

        EditorGUILayout.PropertyField(contentProperty, new GUIContent("Content"));
        EditorGUILayout.PropertyField(amountProperty, new GUIContent("Amount"));
        EditorGUILayout.PropertyField(onlyOnceProperty, new GUIContent("Only Once"));
        EditorGUILayout.PropertyField(isOpenProperty, new GUIContent("Is Open"));
        EditorGUILayout.PropertyField(lockedProperty, new GUIContent("Locked"));
        EditorGUILayout.PropertyField(lockRequirementProperty, new GUIContent("Lock Requirement"));
        EditorGUILayout.PropertyField(requirementAmountProperty, new GUIContent("Requirement Amount"));
        EditorGUILayout.PropertyField(consumesRequirementProperty, new GUIContent("Consumes Requirement"));

        // Dropdown for Locked Dialogue
        //DrawDialogueDropdown(lockedDialogueProperty, "Locked Dialogue");

        // Dropdown for Success Dialogue
        //DrawDialogueDropdown(successDialogueProperty, "Success Dialogue");

        EditorGUILayout.PropertyField(storedOpenProperty, new GUIContent("Stored Open"));
        EditorGUILayout.PropertyField(onIsOpenProperty, new GUIContent("On Is Open Event"));

        DrawDialogueArea(dialogueNames, dialogueFolderPath, "Locked Dialogue", selectedLockedDialogueIndexProperty, lockedDialogueProperty);
        DrawDialogueArea(dialogueNames, dialogueFolderPath, "Unlocked Dialogue", selectedSuccessDialogueIndexProperty, successDialogueProperty);

        serializedObject.ApplyModifiedProperties();



    }


    private void DrawDialogueArea(List<string> dialogueNames, string dialogueFolderPath, string label, SerializedProperty indexProperty, SerializedProperty valueProperty)
    {
        DSInspectorUtility.DrawHeader(label);

        int oldSelectedDialogueIndex = indexProperty.intValue;

        DSDialogueSO oldDialogue = (DSDialogueSO)valueProperty.objectReferenceValue;

        bool isOldDialogueNull = oldDialogue == null;

        string oldDialogueName = isOldDialogueNull ? "" : oldDialogue.DialogueName;

        UpdateIndexOnNamesListUpdate(dialogueNames, indexProperty, oldSelectedDialogueIndex, oldDialogueName, isOldDialogueNull);

        indexProperty.intValue = DSInspectorUtility.DrawPopup("Dialogue", indexProperty, dialogueNames.ToArray());

        string selectedDialogueName = dialogueNames[indexProperty.intValue];

        DSDialogueSO selectedDialogue = DSIOUtility.LoadAsset<DSDialogueSO>(dialogueFolderPath, selectedDialogueName);

        valueProperty.objectReferenceValue = selectedDialogue;

        DSInspectorUtility.DrawDisabledFields(() => valueProperty.DrawPropertyField());
    }
}
