using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public static class DSIOUtility
{

    private static DialogueSystemGraphView graphView;
    private static string graphFileName;
    private static string containerFolderPath;

    private static List<DSGroup> groups;
    private static List<DSNode> nodes;
    private static Dictionary<string, DSDialogueGroupSO> createdDialogueGroups;
    private static Dictionary<string, DSDialogueSO> createdDialogue;


    private static Dictionary<string, DSGroup> loadedGroups;
    private static Dictionary<string, DSNode> loadedNodes;

    public static void Initialize(DialogueSystemGraphView dsGraphview , string graphName)
    {
        graphView = dsGraphview;
        graphFileName = graphName;
        groups = new List<DSGroup>();
        nodes = new List<DSNode>();
        containerFolderPath = $"Assets/DialogueSystem/Dialogues/{graphFileName}";
        createdDialogueGroups = new Dictionary<string, DSDialogueGroupSO>();
        createdDialogue = new Dictionary<string, DSDialogueSO>();
        loadedGroups = new Dictionary<string, DSGroup>();
        loadedNodes = new Dictionary<string, DSNode>();
    }

    public static void Load()
    {

        DSGraphSaveDataSO graphData = LoadAsset<DSGraphSaveDataSO>("Assets/Editor/DialogueSystem/Graphs", graphFileName);

        if(graphData == null)
        {
            EditorUtility.DisplayDialog(
                "Could not Load File",
                "The file at the folling path could not be found : \n\n" + $"Assets/Editor/DialogueSystem/Graphs/{graphFileName} \n\n " +
                "Make sure you chose the right file and it's placed at the folder path mentioned above.",
                "Okay.");

            return;
        }

        DialogueSystemEditorWindow.UpdateFileName(graphData.FileName);

        LoadGroups(graphData.Groups);
        LoadNodes(graphData.Nodes);
        LoadNodesConnections();
        
    }

    private static void LoadNodesConnections()
    {
        foreach(KeyValuePair<string, DSNode> loadedNode in loadedNodes)
        {
            foreach(Port choicePort in loadedNode.Value.outputContainer.Children())
            {
                DSChoiceSaveData choiceData = (DSChoiceSaveData)choicePort.userData;
                if (string.IsNullOrEmpty(choiceData.NodeID))
                {
                    continue;
                }


                DSNode nextNode = loadedNodes[choiceData.NodeID];

                Port nextNodeInputPort = (Port)nextNode.inputContainer.Children().First();

                Edge edge = choicePort.ConnectTo(nextNodeInputPort);

                graphView.AddElement(edge);

                loadedNode.Value.RefreshPorts();

            }
        }
    }

    private static void LoadNodes(List<DSNodeSaveData> nodes)
    {
        foreach(DSNodeSaveData nodeData in nodes)
        {
            List<DSChoiceSaveData> choices = CloneNodeChoices(nodeData.Choices);
            DSNode node = graphView.CreateNode(nodeData.Name, nodeData.DialogueType, nodeData.Position, false);
            node.ID = nodeData.ID;
            node.Choices = choices;
            node.Text = nodeData.Text;
            node.Conditions = CloneNodeConditions(nodeData.Conditions);
            node.BattleConditionParams = CloneBattleConditionParameters(nodeData.BattleConditionParams);
            node.selectedEventCaller = nodeData.EventIndex;

            node.Draw();
            graphView.AddElement(node);
            loadedNodes.Add(node.ID, node);

            if (string.IsNullOrEmpty(nodeData.GroupID))
            {
                continue;
            }

            DSGroup group = loadedGroups[nodeData.GroupID];
            node.Group = group;
            group.AddElement(node);
        }
    }

    private static void LoadGroups(List<DSGroupSaveData> groups)
    {
        
        foreach(DSGroupSaveData groupData in groups)
        {
            DSGroup group = graphView.CreateGroup(groupData.Name, groupData.Position);
            group.ID = groupData.ID;
            loadedGroups.Add(group.ID, group);
        }
    }

    public static void Save()
    {
        CreateStaticFolders();
        GetElementsFromGraphView();
        DSGraphSaveDataSO graphData = CreateAsset<DSGraphSaveDataSO>("Assets/Editor/DialogueSystem/Graphs", $"{graphFileName}Graph");
        graphData.Initialize(graphFileName);


        DSDialogueContainerSO dialogueContainer = CreateAsset<DSDialogueContainerSO>(containerFolderPath, graphFileName);
        dialogueContainer.Initialize(graphFileName);

        SaveGroups(graphData, dialogueContainer);

        SaveNodes(graphData, dialogueContainer);

        SaveAsset(graphData);
        SaveAsset(dialogueContainer);

    }

    private static void SaveNodes(DSGraphSaveDataSO graphData, DSDialogueContainerSO dialogueContainer)
    {
        SerializableDictionary<string, List<string>> groupedNodeNames = new SerializableDictionary<string, List<string>>();
        List<string> ungroupedNodeNames = new List<string>();
        foreach(DSNode node in nodes)
        {
            SaveNodeToGraph(node, graphData);
            SaveNodeToScriptableObject(node, dialogueContainer);
            if(node.Group != null)
            {
                groupedNodeNames.AddItem(node.Group.title, node.DialogueName);
                continue;
            }
                ungroupedNodeNames.Add(node.DialogueName);
           
        }

        UpdateDialogueChoicesConnections();
        UpdateOldGroupedNodes(groupedNodeNames, graphData);
        UpdateOldUngroupedNodes(ungroupedNodeNames, graphData);
    }

    private static void UpdateOldGroupedNodes(SerializableDictionary<string, List<string>> currentGroupedNodeNames, DSGraphSaveDataSO graphData)
    {
        if (graphData.OldGroupedNodeNames != null && graphData.OldGroupedNodeNames.Count != 0)
        {
            foreach(KeyValuePair<string, List<string>> oldGroupedNode in graphData.OldGroupedNodeNames)
            {
                List<string> nodesToRemove = new List<string>();

                if (currentGroupedNodeNames.ContainsKey(oldGroupedNode.Key))
                {
                    nodesToRemove = oldGroupedNode.Value.Except(currentGroupedNodeNames[oldGroupedNode.Key]).ToList();
                }

                foreach(string nodeToRemove in nodesToRemove)
                {
                    RemoveAsset($"{containerFolderPath}/Groups/{oldGroupedNode.Key}/Dialogues", nodeToRemove);
                }
            }
        }
        graphData.OldGroupedNodeNames = new SerializableDictionary<string, List<string>>(currentGroupedNodeNames);
    }

    private static void UpdateOldUngroupedNodes(List<string> currentUngroupedNodeNames, DSGraphSaveDataSO graphData)
    {
        if (graphData.OldUngroupedNodeNames != null && graphData.OldUngroupedNodeNames.Count != 0)
        {
            List<string> nodesToRemove = graphData.OldUngroupedNodeNames.Except(currentUngroupedNodeNames).ToList();

            foreach (string nodeToRemove in nodesToRemove)
            {
                RemoveAsset($"{containerFolderPath}/Global/Dialogues",nodeToRemove);
            }
        }
        graphData.OldUngroupedNodeNames = new List<string>(currentUngroupedNodeNames);
    }


    private static void UpdateDialogueChoicesConnections()
    {
        foreach(DSNode node in nodes)
        {
            DSDialogueSO dialogue = createdDialogue[node.ID];
            for(int choiceIndex = 0; choiceIndex < node.Choices.Count; ++choiceIndex)
            {
                DSChoiceSaveData nodeChoice = node.Choices[choiceIndex];
                if (string.IsNullOrEmpty(nodeChoice.NodeID))
                {
                    continue;
                }


                dialogue.Choices[choiceIndex].NextDialogue = createdDialogue[nodeChoice.NodeID];
                SaveAsset(dialogue);
            }
        }
    }

    private static void SaveNodeToScriptableObject(DSNode node, DSDialogueContainerSO dialogueContainer)
    {
        DSDialogueSO dialogue;
        if(node.Group != null)
        {
            dialogue = CreateAsset<DSDialogueSO>($"{containerFolderPath}/Groups/{node.Group.title}/Dialogues", node.DialogueName);
            dialogueContainer.DialogueGroups.AddItem(createdDialogueGroups[node.Group.ID], dialogue);
        }
        else
        {
            dialogue = CreateAsset<DSDialogueSO>($"{containerFolderPath}/Global/Dialogues", node.DialogueName);
            dialogueContainer.UngroupedDialogues.Add(dialogue);
        }

        dialogue.Initialize(
            node.DialogueName, node.Text, ConvertNodeChoicesToDialogueChoices(node.Choices), node.DialogueType, node.IsStartingNode(), node.Conditions,CloneBattleConditionParameters(node.BattleConditionParams), node.selectedEventCaller
            );

        createdDialogue.Add(node.ID, dialogue);
        SaveAsset(dialogue);
    }


    private static List<BattleCondition> CloneBattleConditionParameters(List<BattleCondition> parameters)
    {
        if(parameters== null || parameters.Count == 0)
        {
            return null;
        }
        List<BattleCondition> conditions = new List<BattleCondition>();

        foreach (BattleCondition condition in parameters)
        {
            BattleCondition conditionData = new BattleCondition(condition);
            conditions.Add(conditionData);
        }

        return conditions;
    }

    private static void SaveNodeToGraph(DSNode node, DSGraphSaveDataSO graphData)
    {
        List<DSChoiceSaveData> choices = CloneNodeChoices(node.Choices);
        DSNodeSaveData nodeData = new DSNodeSaveData()
        {
            Conditions = CloneNodeConditions(node.Conditions),
            BattleConditionParams = CloneBattleConditionParameters(node.BattleConditionParams),
            ID = node.ID,
            Name = node.DialogueName,
            Choices = choices,
            Text = node.Text,
            GroupID = node.Group?.ID,
            DialogueType = node.DialogueType,
            Position = node.GetPosition().position,
            EventIndex = node.selectedEventCaller

        };

        graphData.Nodes.Add(nodeData);
    }

    private static List<DSChoiceSaveData> CloneNodeChoices(List<DSChoiceSaveData> nodeChoices)
    {
        List<DSChoiceSaveData> choices = new List<DSChoiceSaveData>();

        foreach (DSChoiceSaveData choice in nodeChoices)
        {
            DSChoiceSaveData choiceData = new DSChoiceSaveData()
            {
                Text = choice.Text,
                NodeID = choice.NodeID
            };
            choices.Add(choiceData);
        }

        return choices;
    }

    private static List<ConditionResultObject> CloneNodeConditions(List<ConditionResultObject> nodeConditions)
    {
        List<ConditionResultObject> conditions = new List<ConditionResultObject>();

        foreach (ConditionResultObject condition in nodeConditions)
        {
            ConditionResultObject conditionData = new ConditionResultObject();
            conditionData.boolValue = condition.boolValue;
            conditionData.wantedResult = condition.wantedResult;
          
        conditions.Add(conditionData);
        }

        return conditions;
    }

    private static void SaveGroups(DSGraphSaveDataSO graphData, DSDialogueContainerSO dialogueContainer)
    {
        List<string> groupNames = new List<string>();
         foreach(DSGroup group in groups)
        {
            SaveGroupToGraph(group, graphData);
            SaveGroupToScriptableObject(group, dialogueContainer);
            groupNames.Add(group.title);
        }

        UpdateOldGroups(groupNames, graphData);
    }

    private static void UpdateOldGroups(List<string> currentGroupNames, DSGraphSaveDataSO graphData)
    {
        if(graphData.OldGroupNames != null && graphData.OldGroupNames.Count != 0)
        {
            List<string> groupsToRemove = graphData.OldGroupNames.Except(currentGroupNames).ToList();

            foreach(string grouptoremove in groupsToRemove)
            {
                RemoveFolder($"{containerFolderPath}/Groups/{grouptoremove}");
            }
        }
        graphData.OldGroupNames = new List<string>(currentGroupNames);
    }

    public static void RemoveFolder(string fullPath)
    {
        FileUtil.DeleteFileOrDirectory($"{fullPath}/");
        FileUtil.DeleteFileOrDirectory($"{fullPath}.meta");
    }

    private static void SaveGroupToScriptableObject(DSGroup group, DSDialogueContainerSO dialogueContainer)
    {
        string groupName = group.title;
        CreateFolder($"{containerFolderPath}/Groups", groupName);
        CreateFolder($"{containerFolderPath}/Groups/{groupName}", "Dialogues");
        DSDialogueGroupSO dialogueGroup = CreateAsset<DSDialogueGroupSO>($"{containerFolderPath}/Groups/{groupName}", groupName);
        dialogueGroup.Initialize(groupName);
        createdDialogueGroups.Add(group.ID, dialogueGroup);
        dialogueContainer.DialogueGroups.Add(dialogueGroup, new List<DSDialogueSO>());
        SaveAsset(dialogueGroup);
    }

    public static void SaveAsset(UnityEngine.Object asset)
    {
        EditorUtility.SetDirty(asset);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

    private static void SaveGroupToGraph(DSGroup group, DSGraphSaveDataSO graphData)
    {
        DSGroupSaveData groupData = new DSGroupSaveData()
        {
            ID = group.ID,
            Name = group.title,
            Position = group.GetPosition().position
    };


        graphData.Groups.Add(groupData);

    }

    public static T CreateAsset<T>(string path, string assetName) where T : ScriptableObject
    {
        string fullPath = $"{path}/{assetName}.asset";
        T asset = LoadAsset<T>(path,assetName);

        if (asset == null)
        {
            asset = ScriptableObject.CreateInstance<T>();
            AssetDatabase.CreateAsset(asset, fullPath);
        }
        return asset;
    }

    public static T LoadAsset<T>(string path, string assetName) where T : ScriptableObject
    {
        return AssetDatabase.LoadAssetAtPath<T>($"{path}/{assetName}.asset");
    }

    public static void RemoveAsset(string path, string assetName)
    {
        AssetDatabase.DeleteAsset($"{path}/{assetName}.asset");
    }
    public static List<DSDialogueChoiceData> ConvertNodeChoicesToDialogueChoices(List<DSChoiceSaveData> nodeChoices)
    {
        List<DSDialogueChoiceData> dialogueChoices = new List<DSDialogueChoiceData>();
        foreach (DSChoiceSaveData nodeChoice in nodeChoices)
        {
            DSDialogueChoiceData choiceData = new DSDialogueChoiceData()
            {
                Text = nodeChoice.Text
            };

            dialogueChoices.Add(choiceData);
        }

        return dialogueChoices;
    }


    public static void GetElementsFromGraphView()
    {
        graphView.graphElements.ForEach(graphElement =>
        {
            Type groupType = typeof(DSGroup);
            if (graphElement is DSNode node)
            {
                nodes.Add(node);
                return;
            }


            if (graphElement.GetType() == groupType)
            {
                DSGroup group = (DSGroup)graphElement;
                groups.Add(group);
                return;
            }

        });
    }

    private static void CreateStaticFolders()
    {
        CreateFolder("Assets/Editor/DialogueSystem", "Graphs");
        CreateFolder("Assets", "DialogueSystem");
        CreateFolder("Assets/DialogueSystem", "Dialogues");
        CreateFolder($"Assets/DialogueSystem/Dialogues", graphFileName);
        CreateFolder(containerFolderPath, "Global");
        CreateFolder(containerFolderPath, "Groups");
        CreateFolder($"{containerFolderPath}/Global", "Dialogues");
    }

    public static void CreateFolder(string path, string folderName)
    {

        if (AssetDatabase.IsValidFolder($"{path}/{folderName}"))
        {
            return;
        }
        AssetDatabase.CreateFolder( path,  folderName);
    }
}
