using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

public class DSNode : Node
{


    [SerializeField] public List<ConditionResultObject> Conditions = new List<ConditionResultObject>(); // List of conditions

    public string ID { get; set; }
    public string DialogueName { get; set; }
    public List<DSChoiceSaveData> Choices { get; set; }
    public List<string> Text { get; set; } = new List<string>();
    protected DialogueSystemGraphView graphView;
    public DSDialogueType DialogueType { get; set; }

    private Color defaultBackgroundColor;

    public DSGroup Group { get; set; }

    Foldout textFoldout;
    Foldout conditionFoldout;

    public string selectedEventCaller = "None"; // Default dropdown value
    private DropdownField eventCallerDropdown;   // Reference to the dropdown

    public virtual void Initialize(string nodeName, DialogueSystemGraphView dsGraphView, Vector2 position)
    {
        ID = Guid.NewGuid().ToString();
        Text = new List<string> { "Line" };
        graphView = dsGraphView;
        DialogueName = nodeName;
        Choices = new List<DSChoiceSaveData>();
        SetPosition(new Rect(position, Vector2.zero));
        mainContainer.AddToClassList("ds-node__main-container");
        extensionContainer.AddToClassList("ds-node__extension-container");
        defaultBackgroundColor = new Color(29f / 255f, 29f / 255f, 30f / 255f);
    }

    public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
    {
        evt.menu.AppendAction("Disconnect Input Ports", actionEvent =>
        DisconnectInputPorts());
        evt.menu.AppendAction("Disconnect Output Ports", actionEvent =>
        DisconnectOutputPorts());

        evt.menu.AppendAction("Duplicate", actionEvent =>
        {
            DuplicateNode();
        });
        base.BuildContextualMenu(evt);
    }


    public void DuplicateNode(bool changeName = true)
    {
        // Clone the current node
        string newName = DialogueName + (changeName ? "2" : "");
        DSNode duplicateNode = graphView.CreateNode(newName, DialogueType, GetPosition().position + Vector2.right * 200 + Vector2.down * 200, false); // Create an instance of the same type
        duplicateNode.Text = new List<string>(Text);
        duplicateNode.Choices = new List<DSChoiceSaveData>(Choices);
        duplicateNode.Conditions = new List<ConditionResultObject>(Conditions);
        duplicateNode.DialogueType = DialogueType;
        duplicateNode.selectedEventCaller = selectedEventCaller;

        duplicateNode.Draw();
        graphView.AddElement(duplicateNode); // Add the duplicate to the graph view or container

        if (Group != null)
        {
            Group.AddElement(duplicateNode);
        }
    }

    public virtual void Draw()
    {
        TextField dialogueNameTextField = DSElementUtility.CreateTextField(DialogueName, null, callback =>
        {
            TextField target = (TextField)callback.target;
            target.value = callback.newValue.RemoveWhitespaces().RemoveSpecialCharacters();
            if (string.IsNullOrEmpty(target.value))
            {
                if (!string.IsNullOrEmpty(DialogueName))
                {
                    ++graphView.NameErrorsAmount;
                }
            }
            else
            {
                if (string.IsNullOrEmpty(DialogueName))
                {
                    --graphView.NameErrorsAmount;
                }
            }
            if (Group == null)
            {
                graphView.RemovedUngroupedNode(this);
                DialogueName = target.value;
                graphView.AddUngroupedNode(this);
                return;
            }

            DSGroup currentGroup = Group;
            graphView.RemoveGroupedNode(this, Group);
            DialogueName = callback.newValue;
            graphView.AddGroupedNode(this, currentGroup);
        });

        Button addTextButton = new Button(() =>
        {
            Text.Add(string.Empty); // Add empty entry
            AddTextElementUI(Text.Count - 1); // Draw new field
        })
        {
            text = "Add Text"
        };

        mainContainer.Insert(1, addTextButton);

        dialogueNameTextField.AddClasses("ds-node__text-field", "ds-node__filename-text-field", "ds-node__text-field__hidden");
        titleContainer.Insert(0, dialogueNameTextField);

        Port inputPort = InstantiatePort(Orientation.Horizontal, UnityEditor.Experimental.GraphView.Direction.Input, Port.Capacity.Multi, typeof(bool));
        inputPort.portName = "Dialogue Connection";
        inputContainer.Add(inputPort);

        VisualElement customDataContainer = new VisualElement();
        customDataContainer.AddClasses("ds-node__custom-data-container");

        // Text Foldout for list of texts
        textFoldout = DSElementUtility.CreateFoldout("Dialogue Text");
        AddTextListUI();
        customDataContainer.Add(textFoldout);

        // Conditions Foldout (initially hidden)
        conditionFoldout = DSElementUtility.CreateFoldout("Conditions");
        conditionFoldout.style.display = Conditions.Count>0?DisplayStyle.Flex:DisplayStyle.None; // Hide initially
        AddConditionListUI();

        Button addConditionButton = new Button(() =>
        {
            ConditionResultObject newCondition = new ConditionResultObject();
            Conditions.Add(newCondition);
            AddConditionElementUI(newCondition);

            // Make the condition foldout visible and expanded when a condition is added
            conditionFoldout.style.display = DisplayStyle.Flex;
            conditionFoldout.value = true; // Expand the foldout
        })
        {
            text = "Add Condition"
        };

        customDataContainer.Add(conditionFoldout);
        mainContainer.Insert(1, addConditionButton);

        // Event Caller Dropdown
        VisualElement eventCallerContainer = new VisualElement();
        eventCallerContainer.AddToClassList("ds-node__event-caller-container");
        AddEventCallerUI(eventCallerContainer);
        customDataContainer.Add(eventCallerContainer);

        extensionContainer.Add(customDataContainer);

        RefreshExpandedState();
    }

    private void AddTextListUI()
    {
        // Clear existing foldout
        textFoldout.Clear();

        // Render existing text entries
        for (int i = 0; i < Text.Count; i++)
        {
            int index = i;
            AddTextElementUI(index);
        }
    }

    private void AddTextElementUI(int index)
    {
        // Create a container for the text field and button
        VisualElement textContainer = new VisualElement();
        textContainer.AddToClassList("ds-node__text-container");

        // Apply flexbox styling for horizontal layout
        textContainer.style.flexDirection = FlexDirection.Row;
        textContainer.style.alignItems = Align.Center;
        textContainer.style.justifyContent = Justify.FlexStart;

        // Text Field for dialogue text
        TextField textField = new TextField()
        {
            value = Text[index]
        };
        textField.AddToClassList("ds-node__text-field");
        textField.style.flexGrow = 1; // Allow the text field to grow and occupy remaining space
        textField.style.minWidth = Length.Percent(80); // Reserve space for the button (20% for the button)
        textField.RegisterValueChangedCallback(evt =>
        {
            Text[index] = evt.newValue;
        });

        // Remove Button
        Button removeButton = new Button(() =>
        {
            Text.RemoveAt(index); // Remove from list
            textFoldout.Remove(textContainer); // Remove from UI
            AddTextListUI(); // Refresh UI
        })
        {
            text = "X"
        };
        removeButton.AddToClassList("ds-node__button");
        removeButton.style.flexShrink = 0; // Prevent the button from shrinking

        // Add elements to the container
        textContainer.Add(textField);
        textContainer.Add(removeButton);

        // Add the container to the foldout
        textFoldout.Add(textContainer);
    }

    private void AddConditionListUI()
    {
        foreach (ConditionResultObject condition in Conditions)
        {
            AddConditionElementUI(condition);
        }
    }

    private void AddConditionElementUI(ConditionResultObject condition)
    {
        VisualElement conditionContainer = new VisualElement();
        conditionContainer.AddClasses("ds-node__condition-container");

        // ObjectField for BoolValue
        ObjectField boolValueField = new ObjectField("Bool Value")
        {
            objectType = typeof(BoolValue),
            value = condition.boolValue
        };

        boolValueField.RegisterValueChangedCallback(evt =>
        {
            condition.boolValue = (BoolValue)evt.newValue;
        });

        // Toggle for Wanted Result
        Toggle wantedResultToggle = new Toggle("Wanted Result")
        {
            value = condition.wantedResult
        };

        wantedResultToggle.RegisterValueChangedCallback(evt =>
        {
            condition.wantedResult = evt.newValue;
        });

        // Remove Button
        Button removeButton = new Button(() =>
        {
            // Remove the condition from the list and update the UI
            Conditions.Remove(condition);
            conditionFoldout.Remove(conditionContainer);
            conditionFoldout.style.display = Conditions.Count > 0 ? DisplayStyle.Flex : DisplayStyle.None;
        })
        {
            text = "Remove"
        };
        removeButton.AddToClassList("ds-node__condition-remove-button");

        // Add elements to the condition container
        conditionContainer.Add(boolValueField);
        conditionContainer.Add(wantedResultToggle);
        conditionContainer.Add(removeButton);

        // Add condition container to the foldout
        conditionFoldout.Add(conditionContainer);
    }

    private void AddEventCallerUI(VisualElement container)
    {
        // Dropdown options: "None" + numbers 0 to 15
        List<string> eventCallerOptions = new List<string> { "None" };
        for (int i = 0; i <= 15; i++)
        {
            eventCallerOptions.Add(i.ToString());
        }

        // Create the DropdownField
        eventCallerDropdown = new DropdownField("Event Caller", eventCallerOptions, 0);
        eventCallerDropdown.RegisterValueChangedCallback(evt =>
        {
            selectedEventCaller = evt.newValue;
            Debug.Log($"Selected Event Caller: {selectedEventCaller}");
        });
        eventCallerDropdown.SetValueWithoutNotify(selectedEventCaller);
        // Add to the container
        container.Add(eventCallerDropdown);
    }

    public void DisconnectAllPorts()
    {
        DisconnectInputPorts();
        DisconnectOutputPorts();
    }


    private void DisconnectInputPorts()
    {
        DisconnectPorts(inputContainer);

    }

    private void DisconnectOutputPorts()
    {
        DisconnectPorts(outputContainer);

    }



    private void DisconnectPorts(VisualElement container)
    {
        foreach (Port port in container.Children())
        {
            if (!port.connected)
            {
                continue;
            }


            graphView.DeleteElements(port.connections);
        }
    }


    public bool IsStartingNode()
    {
        Port inputPort = (Port)inputContainer.Children().First();

        return !inputPort.connected;

    }
    public void SetErrorStyle(Color color)
    {
        mainContainer.style.backgroundColor = color;
    }


    public void ResetStyle()
    {
        mainContainer.style.backgroundColor = defaultBackgroundColor;
    }
}



