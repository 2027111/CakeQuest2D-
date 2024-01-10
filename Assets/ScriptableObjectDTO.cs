using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
[System.Serializable]
public class ScriptableObjectDTO
{
    public string typeName; // To store the type name
    public string jsonData; // To store the serialized JSON data
    public string Id; // To store the unique identifier

    public ScriptableObjectDTO(YourScriptableObject scriptableObject, Guid objectId)
    {
        typeName = scriptableObject.GetType().FullName;
        jsonData = JsonUtility.ToJson(scriptableObject);
        Id = objectId.ToString();
    }

    public void ApplyData(YourScriptableObject scriptableObject)
    {
        // Ensure the type matches before deserializing
        if (scriptableObject.GetType().FullName.Equals(typeName))
        {
            // Log JSON data

            // Create a temporary ScriptableObject copy
            YourScriptableObject tempCopy = ScriptableObject.CreateInstance(scriptableObject.GetType()) as YourScriptableObject;

            // Deserialize JSON data into the temporary copy
            JsonUtility.FromJsonOverwrite(jsonData, tempCopy);
            tempCopy.Id = scriptableObject.Id;

            // Compare and apply only string fields from the temporary copy to the target ScriptableObject
            ApplyStringFields(scriptableObject, tempCopy);

            // Log information about the deserialization
        }
        else
        {
            Debug.LogError($"Type mismatch when deserializing ScriptableObject. Expected {typeName}, got {scriptableObject.GetType().FullName}");
        }
    }

    private void ApplyStringFields(YourScriptableObject target, YourScriptableObject source)
    {
        // Get all fields of the target and source objects
        System.Reflection.FieldInfo[] targetFields = target.GetType().GetFields();
        System.Reflection.FieldInfo[] sourceFields = source.GetType().GetFields();

        foreach (var sourceField in sourceFields)
        {
            // Check if the field type is enum, numeric, boolean, or string
            if (IsSupportedFieldType(sourceField.FieldType))
            {
                // Find the corresponding field in the target object
                var matchingField = System.Array.Find(targetFields, field => field.Name == sourceField.Name);

                // Apply the value from the source to the target
                if (matchingField != null)
                {
                    matchingField.SetValue(target, sourceField.GetValue(source));
                }
            }
        }
    }

    private bool IsSupportedFieldType(Type fieldType)
    {
        // Check if the field type is an enum, a numeric type, boolean, or string
        return fieldType.IsEnum || fieldType == typeof(int) || fieldType == typeof(float) ||
               fieldType == typeof(double) || fieldType == typeof(bool) || fieldType == typeof(string);
    }
}

[System.Serializable]
public class ScriptableObjectListWrapper
{
    public List<ScriptableObjectDTO> objects;
}
