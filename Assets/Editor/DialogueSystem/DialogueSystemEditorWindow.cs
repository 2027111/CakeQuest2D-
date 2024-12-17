using System;
using System.IO;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

public class DialogueSystemEditorWindow : EditorWindow
{

    private readonly string defaultFileName = "DialoguesFileName";
    private static TextField fileNameTextField;
    private Button saveButton;
    private DialogueSystemGraphView graphView;

    [MenuItem("Window/Dialogue System/Dialogue Graph")]
    public static void Open()
    {
        GetWindow<DialogueSystemEditorWindow>("Dialogue Graph");

    }

    private void CreateGUI()
    {
        AddGraphView();
        AddStyles();
        AddToolBar();
    }

    private void AddToolBar()
    {
        Toolbar toolbar = new Toolbar();

        fileNameTextField = DSElementUtility.CreateTextField(defaultFileName, "File Name", callback =>
        {

            fileNameTextField.value = callback.newValue.RemoveWhitespaces().RemoveSpecialCharacters();

        });
        saveButton = DSElementUtility.CreateButton("Save", ()=> Save());


        Button loadButton = DSElementUtility.CreateButton("Load", () => Load());

        Button clearButton = DSElementUtility.CreateButton("Clear", () => Clear());



        Button resetButton = DSElementUtility.CreateButton("Reset", () => ResetGraph());

        toolbar.Add(fileNameTextField);
        toolbar.Add(saveButton);
        toolbar.Add(loadButton);
        toolbar.Add(clearButton);
        toolbar.Add(resetButton);
        toolbar.AddStyleSheets("Dialogue System/DSToolbarStyles.uss");

        rootVisualElement.Add(toolbar);
    }

    private void Load()
    {

        string filePath = EditorUtility.OpenFilePanel("Dialogue Graphs", "Assets/Editor/DialogueSystem/Graphs", "asset");
        if (string.IsNullOrEmpty(filePath))
        {
            return;
        }
        Clear();
        DSIOUtility.Initialize(graphView, Path.GetFileNameWithoutExtension(filePath));
        DSIOUtility.Load();
    }

    private void ResetGraph()
    {
        Clear();
        UpdateFileName(defaultFileName);
    }
    public static void UpdateFileName(string newFileName)
    {
        fileNameTextField.value = newFileName;
    }

    private void Clear()
    {
        graphView.ClearGraph();
        
    }

    private void Save()
    {

        if (string.IsNullOrEmpty(fileNameTextField.value))
        {
            EditorUtility.DisplayDialog(
                "Invalid File name.", "Please ensure the file name you've typed in is Valid.",
                "Okay."
                );
            return;
        }
        DSIOUtility.Initialize(graphView, fileNameTextField.value);
        DSIOUtility.Save();
    }

    private void AddStyles()
    {
        rootVisualElement.AddStyleSheets("Dialogue System/DSVariables.uss");
    }
    private void AddGraphView()
    {
        graphView = new DialogueSystemGraphView(this);

        graphView.StretchToParentSize();
        rootVisualElement.Add(graphView);
    }




    public void EnableSaving()
    {
        saveButton.SetEnabled(true);


    }



    public void DisableSaving()
    {

        saveButton.SetEnabled(false);


    }
}
