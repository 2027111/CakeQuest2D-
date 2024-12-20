using System;
using System.IO;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

public class DialogueSystemEditorWindow : EditorWindow
{

    private readonly string defaultFileName = "DialoguesFileName";
    private bool graphLoaded = false;
    private static TextField fileNameTextField;
    private Button saveButton;
    private Button miniMapButton;
    private Button addToGraphButton;
    private Button deleteGraphFileButton;
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

        saveButton = DSElementUtility.CreateButton("Save", () => Save());
        AddIconToButton(saveButton, "save_icon.png");

        Button loadButton = DSElementUtility.CreateButton("Load", () => Load());
        AddIconToButton(loadButton, "load_icon.png");

        Button clearButton = DSElementUtility.CreateButton("Clear", () => Clear());
        AddIconToButton(clearButton, "clear_icon.png");

        Button resetButton = DSElementUtility.CreateButton("Reset", () => ResetGraph());
        AddIconToButton(resetButton, "reset_icon.png");

        miniMapButton = DSElementUtility.CreateButton("Minimap", () => ToggleMiniMap());
        AddIconToButton(miniMapButton, "minimap_icon.png");


        addToGraphButton = DSElementUtility.CreateButton("Add To Current Graph", () => Load(false));

        deleteGraphFileButton = DSElementUtility.CreateButton("Delete Graph Files", () => DeleteGraphFile());
        AddIconToButton(deleteGraphFileButton, "delete_icon.png");

        toolbar.Add(fileNameTextField);
        toolbar.Add(saveButton);
        toolbar.Add(loadButton);
        toolbar.Add(clearButton);
        toolbar.Add(resetButton);
        toolbar.Add(addToGraphButton);
        toolbar.Add(miniMapButton);
        toolbar.Add(deleteGraphFileButton);
        toolbar.AddStyleSheets("Dialogue System/DSToolbarStyles.uss");

        ToggleAddToGraphButton();
        rootVisualElement.Add(toolbar);
    }

    private void AddIconToButton(Button button, string iconName)
    {
        // Create a container to hold both the icon and the text
        VisualElement buttonContent = new VisualElement();
        buttonContent.style.flexDirection = FlexDirection.Row; // Arrange icon and text horizontally
        buttonContent.style.alignItems = Align.Center; // Center the icon and text vertically
        buttonContent.style.justifyContent = Justify.FlexStart; // Ensure elements start at the left
        buttonContent.style.marginTop = 5;
        // Load the icon as a VisualElement (Image)
        Image icon = new Image();
        icon.image = AssetDatabase.LoadAssetAtPath<Texture2D>($"Assets/Editor/Icons/{iconName}");
        icon.style.width = 20;  // Set the width of the icon
        icon.style.height = 20; // Set the height of the icon
        icon.style.marginRight = 8; // Add some space between the icon and the text

        // Create a label for the text
        Label label = new Label(button.text);
        button.text = "";  // Remove the default text

        // Add the icon and label to the container
        buttonContent.Add(icon);
        buttonContent.Add(label);

        // Replace the button's content with the new container
        button.Clear();
        button.Add(buttonContent);

        // Optional: Adjust the size of the button container (button) to ensure it has enough space for both icon and text
    }

    private void DeleteGraphFile()
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
        DSIOUtility.Delete(ResetGraph);
    }

    private void ToggleMiniMap()
    {
        graphView.ToggleMiniMap();
        miniMapButton.ToggleInClassList("ds-toolbar__button__selected");
    }

    private void Load(bool clear = true)
    {
        graphView.ClearSelection();
        string currentFileName = fileNameTextField.value;
        string filePath = EditorUtility.OpenFilePanel("Dialogue Graphs", "Assets/Editor/DialogueSystem/Graphs", "asset");
        if (string.IsNullOrEmpty(filePath))
        {
            return;
        }
        if (clear) { Clear(); }
        DSIOUtility.Initialize(graphView, Path.GetFileNameWithoutExtension(filePath));
        DSIOUtility.Load();

        graphLoaded = true;
        ToggleAddToGraphButton();
        if (!clear)
        { UpdateFileName(currentFileName); }
    }


    public void ToggleAddToGraphButton()
    {
            addToGraphButton.SetEnabled(graphLoaded);
            deleteGraphFileButton.SetEnabled(graphLoaded);
            fileNameTextField.SetEnabled(!graphLoaded);

    }
    private void ResetGraph()
    {
        Clear();
        UpdateFileName(defaultFileName);
        graphLoaded = false;
        ToggleAddToGraphButton();
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
        graphLoaded = true;
        ToggleAddToGraphButton();
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
