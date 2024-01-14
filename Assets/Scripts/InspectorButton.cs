using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
[CustomEditor(typeof(GameSaveManager))]
public class InspectorButton : Editor
{

    public override void OnInspectorGUI()
    {
        GameSaveManager manager = (GameSaveManager)target;
        DrawDefaultInspector();

        if(GUILayout.Button("Generate Translation Files"))
        {
            manager.CreateSavePath();
            manager.SaveEverything();
        }
    }
}