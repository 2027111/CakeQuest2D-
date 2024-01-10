using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
[CustomEditor(typeof(DialogueManager))]
public class InspectorButton : Editor
{

    public override void OnInspectorGUI()
    {
        DialogueManager manager = (DialogueManager)target;
        DrawDefaultInspector();

        if(GUILayout.Button("Generate Translation Files"))
        {
            manager.CreateSavePath();
            manager.SaveEverything();
        }
    }
}