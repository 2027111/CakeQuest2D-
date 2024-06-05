using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class MenuNames : ScriptableObject
{
    public string newGame = "New Game";
    public string continueGame = "Continue";
    public string leaveGame = "Leave";
    public string settings = "Settings";
    public string inventory = "Inventory";
    public string pause = "Pause";
    public string use = "Use";
    public string returnToTitle = "Return to Title";
    public string resume = "Resume";
    public string save = "Save";

    // Method to get the value of a string field by its name
    public string GetStringValue(string fieldName)
    {
        var field = GetType().GetField(fieldName);

        if (field != null && field.FieldType == typeof(string))
        {
            return field.GetValue(this) as string;
        }
        else
        {
            Debug.LogWarning("Field not found or not of type string: " + fieldName);
            return null;
        }
    }

    // Method to get a list of all field names
    public List<string> GetAllFieldNames()
    {
        List<string> fieldNames = new List<string>();

        foreach (var fieldInfo in GetType().GetFields())
        {
            if (fieldInfo.FieldType == typeof(string))
            {
                fieldNames.Add(fieldInfo.Name);
            }
        }

        return fieldNames;
    }
}
