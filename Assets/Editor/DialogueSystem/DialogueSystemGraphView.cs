using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public class DialogueSystemGraphView : GraphView
{

    private DSSearchWindow searchWindow;
    private DialogueSystemEditorWindow editorWindow;

    private SerializableDictionary<string, DSNodeErrorData> ungroupedNodes;
    private SerializableDictionary<string, DSGroupErrorData> groups;
    private SerializableDictionary<Group, SerializableDictionary<string, DSNodeErrorData>> groupedNodes;

    private int nameErrorsAmount;
    public int NameErrorsAmount
    {
        get
        {
            return nameErrorsAmount;
        }

        set
        {
            nameErrorsAmount = value;
            if(nameErrorsAmount == 0)
            {
                editorWindow.EnableSaving();
            }

            if(nameErrorsAmount == 1)
            {
                editorWindow.DisableSaving();
            }
        }
    }



    public DialogueSystemGraphView(DialogueSystemEditorWindow dialogueSystemEditorWindow)
    {
        editorWindow = dialogueSystemEditorWindow;
        ungroupedNodes = new SerializableDictionary<string, DSNodeErrorData>();
        groupedNodes = new SerializableDictionary<Group, SerializableDictionary<string, DSNodeErrorData>>();
        groups = new SerializableDictionary<string, DSGroupErrorData>();
        AddManipulators();
        AddGridBackground();
        AddSearchWindow();

        OnGroupRenamed();
        OnElementsDeleted();
        OnGroupElementsAdded();
        OnGroupElementsRemoved();
        OnGraphViewChanges();

        AddStyles();

    }

    private void AddSearchWindow()
    {
        if (searchWindow == null)
        {
            searchWindow = ScriptableObject.CreateInstance<DSSearchWindow>();
            searchWindow.Initialize(this);
        }

        nodeCreationRequest = context => SearchWindow.Open(new SearchWindowContext(context.screenMousePosition), searchWindow);
    }

    public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
    {

        List<Port> compatiblePorts = new List<Port>();

        ports.ForEach(port =>
        {
            if (startPort == port)
            {
                return;
            }


            if (startPort.node == port.node)
            {
                return;
            }



            if (startPort.direction == port.direction)
            {
                return;
            }


            compatiblePorts.Add(port);
        });

        return compatiblePorts;
    }

    private void AddManipulators()
    {
        SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);
        this.AddManipulator(new SelectionDragger());
        this.AddManipulator(new RectangleSelector());
        this.AddManipulator(CreateNodeContextualMenu("Add Node (Single Choice)", DSDialogueType.SingleChoice));
        this.AddManipulator(CreateNodeContextualMenu("Add Node (Multiple Choice)", DSDialogueType.MultipleChoice));
        this.AddManipulator(new ContentDragger());


        this.AddManipulator(CreateNodeContextualMenu());
    }
    private IManipulator CreateNodeContextualMenu()
    {
        ContextualMenuManipulator contextualMenuManipulator = new ContextualMenuManipulator(

            menuEvent => menuEvent.menu.AppendAction("Add Group", actionEvent => CreateGroup("Dialogue Group", GetLocalMousePosition(actionEvent.eventInfo.localMousePosition)))
            ); ;




        return contextualMenuManipulator;
    }


    public DSGroup CreateGroup(string title, Vector2 localMousePosition)
    {
        DSGroup group = new DSGroup(title, localMousePosition);


        AddGroup(group);
        AddElement(group);
        foreach(GraphElement selectedElement in selection)
        {
            if(!(selectedElement is DSNode))
            {
                continue;
            }
            DSNode node = (DSNode)selectedElement;
            group.AddElement(node);

        }
        return group;
    }

    public void AddGroup(DSGroup group)
    {

        string groupName = group.title.ToLower();

        if (!groups.ContainsKey(groupName))
        {
            DSGroupErrorData groupErrorData = new DSGroupErrorData();
            groupErrorData.Groups.Add(group);
            if (!groups.ContainsKey(groupName))
                groups.Add(groupName, groupErrorData);
            return;
        }

        List<DSGroup> groupList = groups[groupName].Groups;

        groupList.Add(group);

        Color errorColor = groups[groupName].ErrorData.Color;
        if (groupList.Count == 2)
        {

            groupList[0].SetErrorStyle(errorColor);

        }
        group.SetErrorStyle(errorColor);

    }

    private void RemoveGroup(DSGroup group)
    {
        string oldGroupName = group.OldTitle.ToLower();
        List<DSGroup> groupList = groups[oldGroupName].Groups;
        groupList.Remove(group);
        group.ResetStyle();

        if(groupList.Count == 1)
        {
            groupList[0].ResetStyle();
            return;
        }

        if(groupList.Count == 0)
        {
            groups.Remove(oldGroupName);
        }
    }

    private IManipulator CreateNodeContextualMenu(string actionTitle, DSDialogueType dialogueType)
    {
        ContextualMenuManipulator contextualMenuManipulator = new ContextualMenuManipulator(

            menuEvent => menuEvent.menu.AppendAction(actionTitle, actionEvent => AddElement(CreateNode("DialogueName",dialogueType, GetLocalMousePosition(actionEvent.eventInfo.localMousePosition))))
            ); ;




        return contextualMenuManipulator;
    }

    private void AddStyles()
    {

        this.AddStyleSheets("Dialogue System/DSGraphViewStyles.uss", "Dialogue System/DSNodeStyle.uss");
    }

    private void AddGridBackground()
    {
        GridBackground grid = new GridBackground();
        grid.StretchToParentSize();
        Insert(0, grid);
    }

    public DSNode CreateNode(string nodeName, DSDialogueType dialogueType, Vector2 position, bool shouldDraw = true)
    {
        Type noteType = Type.GetType($"DS{dialogueType}Node");
        DSNode node = (DSNode)Activator.CreateInstance(noteType);
        node.Initialize(nodeName, this, position);
        if (shouldDraw)
        {
            node.Draw();
        }
        AddUngroupedNode(node);
        return node;
    }

    private void OnGroupRenamed()
    {
        groupTitleChanged = (group, newTitle) =>
        {
            DSGroup dsGroup = (DSGroup)group;




            dsGroup.title = newTitle.RemoveWhitespaces().RemoveSpecialCharacters();
            if (string.IsNullOrEmpty(dsGroup.title))
            {
                if (!string.IsNullOrEmpty(dsGroup.OldTitle))
                {
                    ++NameErrorsAmount;
                }
            }
            else
            {
                if (string.IsNullOrEmpty(dsGroup.OldTitle))
                {
                    --NameErrorsAmount;
                }

            }
            RemoveGroup(dsGroup);
            dsGroup.OldTitle = dsGroup.title;
            AddGroup(dsGroup);
        };
    }


    private void OnElementsDeleted()
    {
        deleteSelection = (operationName, askUser) =>
        {
            Type groupType = typeof(DSGroup);
            Type edgeType = typeof(Edge);
            List<DSGroup> groupsToDelete = new List<DSGroup>();
            List<Edge> edgeToDelete = new List<Edge>();
            List<DSNode> nodesToDelete = new List<DSNode>();
            foreach (GraphElement element in selection)
            {
                if (element is DSNode node)
                {
                    nodesToDelete.Add(node);
                    continue;
                }

                if (element.GetType() == edgeType)
                {
                    Edge edge = (Edge)element;

                    edgeToDelete.Add(edge);
                }

                if (element.GetType() != groupType)
                {
                    continue;
                }

                DSGroup group = (DSGroup)element;
                groupsToDelete.Add(group);
            }

            foreach (DSGroup group in groupsToDelete)
            {
                List<DSNode> groupNodes = new List<DSNode>();
                foreach (GraphElement groupElement in group.containedElements)
                {
                    if (!(groupElement is DSNode))
                    {
                        continue;
                    }



                    DSNode groupNode = (DSNode)groupElement;

                    groupNodes.Add(groupNode);
                }

                group.RemoveElements(groupNodes);

                RemoveGroup(group);
                RemoveElement(group);
            }
            DeleteElements(edgeToDelete);

            foreach (DSNode node in nodesToDelete)
            {
                if (node.Group != null)
                {
                    node.Group.RemoveElement(node);
                }
                RemovedUngroupedNode(node);
                node.DisconnectAllPorts();
                RemoveElement(node);
            }


        };
    }

    private void OnGraphViewChanges()
    {
        graphViewChanged = (changes) =>
        {
            if(changes.edgesToCreate != null)
            {

                foreach(Edge edge in changes.edgesToCreate)
                {
                    DSNode nextNode = (DSNode)edge.input.node;
                    DSChoiceSaveData choiceData = (DSChoiceSaveData)edge.output.userData;
                    choiceData.NodeID = nextNode.ID;
                }





            }

            if (changes.elementsToRemove != null)
            {
                Type edgeType = typeof(Edge);

                foreach (GraphElement element in changes.elementsToRemove)
                {
                    if(element.GetType() != edgeType)
                    {
                        continue;
                    }
                    Edge edge = (Edge)element;
                    DSChoiceSaveData choiceData = (DSChoiceSaveData)edge.output.userData;
                    choiceData.NodeID = "";
                }
            }

            return changes;
        };
    }

    public void AddUngroupedNode(DSNode node)
    {
        string nodeName = node.DialogueName.ToLower();
        if (!ungroupedNodes.ContainsKey(nodeName))
        {
            DSNodeErrorData nodeErrorData = new DSNodeErrorData();
            nodeErrorData.Nodes.Add(node);
            ungroupedNodes.Add(nodeName, nodeErrorData);
            return;
        }

        List<DSNode> ungroupedNodesList = ungroupedNodes[nodeName].Nodes;

        ungroupedNodesList.Add(node);

        Color errorColor = ungroupedNodes[nodeName].ErrorData.Color;
        if (ungroupedNodesList.Count == 2)
        {
            ++NameErrorsAmount;
            ungroupedNodesList[0].SetErrorStyle(errorColor);

        }
        node.SetErrorStyle(errorColor);


    }


    public void RemovedUngroupedNode(DSNode node)
    {

        string nodeName = node.DialogueName.ToLower();



        List<DSNode> ungroupedNodesList = ungroupedNodes[nodeName].Nodes;

        ungroupedNodesList.Remove(node);
        node.ResetStyle();



        if (ungroupedNodesList.Count == 1)
        {
            --NameErrorsAmount;
            ungroupedNodesList[0].ResetStyle();
            return;
        }

        if (ungroupedNodesList.Count == 0)
        {
            ungroupedNodes.Remove(nodeName);
        }
    }


    private void OnGroupElementsAdded()
    {
        elementsAddedToGroup = (group, elements) =>
        {
            foreach (GraphElement element in elements)
            {
                if (!(element is DSNode))
                {
                    continue;
                }

                DSGroup nodeGroup = (DSGroup)group;
                DSNode node = (DSNode)element;
                RemovedUngroupedNode(node);
                AddGroupedNode(node, nodeGroup);
            }
        };
    }


    private void OnGroupElementsRemoved()
    {
        elementsRemovedFromGroup = (group, elements) =>
        {
            foreach (GraphElement element in elements)
            {
                if (!(element is DSNode))
                {
                    continue;
                }

                DSGroup nodeGroup = (DSGroup)group;
                DSNode node = (DSNode)element;
                RemoveGroupedNode(node, nodeGroup);
                AddUngroupedNode(node);
            }
        };
    }

    public void RemoveGroupedNode(DSNode node, DSGroup group)
    {
        string nodeName = node.DialogueName.ToLower();

        node.Group = null;


        List<DSNode> groupedNodesList = groupedNodes[group][nodeName].Nodes;

        groupedNodesList.Remove(node);
        node.ResetStyle();


        if (groupedNodesList.Count == 1)
        {
            --NameErrorsAmount;
            groupedNodesList[0].ResetStyle();
            return;
        }

        if (groupedNodesList.Count == 0)
        {
            groupedNodes[group].Remove(nodeName);


            if (groupedNodes[group].Count == 0)
            {
                groupedNodes.Remove(group);
            }
        }



    }

    public void AddGroupedNode(DSNode node, DSGroup group)
    {
        string nodeName = node.DialogueName.ToLower();
        node.Group = group;

        if (!groupedNodes.ContainsKey(group))
        {
            groupedNodes.Add(group, new SerializableDictionary<string, DSNodeErrorData>());
        }


        if (!groupedNodes[group].ContainsKey(nodeName))
        {
            DSNodeErrorData nodeErrorData = new DSNodeErrorData();
            nodeErrorData.Nodes.Add(node);
            groupedNodes[group].Add(nodeName, nodeErrorData);
            return;

        }

        List<DSNode> groupedNodesList = groupedNodes[group][nodeName].Nodes;
        groupedNodesList.Add(node);
        Color errorColor = groupedNodes[group][nodeName].ErrorData.Color;
        node.SetErrorStyle(errorColor);
        if (groupedNodesList.Count == 2)
        {
            ++NameErrorsAmount;
            groupedNodesList[0].SetErrorStyle(errorColor);
        }



    }

    public Vector2 GetLocalMousePosition(Vector2 mousePosition, bool isSearchWindow = false)
    {
        Vector2 worldMousePosition = mousePosition;

        if (isSearchWindow)
        {
            worldMousePosition = editorWindow.rootVisualElement.ChangeCoordinatesTo(editorWindow.rootVisualElement.parent, mousePosition - editorWindow.position.position);
        }

        Vector2 localMousePosition = contentViewContainer.WorldToLocal(worldMousePosition);

        return localMousePosition;
    }


    public void ClearGraph()
    {

        graphElements.ForEach(graphElement => RemoveElement(graphElement));
        groups.Clear();
        groupedNodes.Clear();
        ungroupedNodes.Clear();

        NameErrorsAmount = 0;
    }
}
