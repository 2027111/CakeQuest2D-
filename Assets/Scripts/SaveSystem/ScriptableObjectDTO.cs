using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
[System.Serializable]
public class ScriptableObjectDTO
{
    public string typeName; // To store the type name
    public string objectName; // To store the scriptable object name
    public string uid; // To store the uid
    public string jsonData; // To store the serialized JSON data
    public int HashCode; // To store the unique identifier

    public ScriptableObjectDTO(SavableObject scriptableObject)
    {
        typeName = scriptableObject.GetType().FullName;
        objectName = scriptableObject.name;
        uid = scriptableObject.UID;
        jsonData = JsonUtility.ToJson(scriptableObject, true);
        HashCode = scriptableObject.GetHashCode();
    }

    public void ApplyData(SavableObject scriptableObject)
    {
        // Ensure the type matches before deserializing
        if (scriptableObject.GetType().FullName.Equals(typeName))
        {
            // Log JSON data

            // Create a temporary ScriptableObject copy
            SavableObject tempCopy = SavableObject.CreateInstance(scriptableObject.GetType()) as SavableObject;

            // Deserialize JSON data into the temporary copy
            JsonUtility.FromJsonOverwrite(jsonData, tempCopy);

            // Compare and apply only string fields from the temporary copy to the target ScriptableObject
            scriptableObject.ApplyData(tempCopy);
            //ApplyStringFields(scriptableObject, tempCopy);

            // Log information about the deserialization
        }
        else
        {
            Debug.LogError($"Type mismatch when deserializing ScriptableObject. Expected {typeName}, got {scriptableObject.GetType().FullName}");
        }
    }

    private void ApplyStringFields(SavableObject target, SavableObject source)
    {
        // Get all fields of the target and source objects
        System.Reflection.FieldInfo[] targetFields = target.GetType().GetFields();
        System.Reflection.FieldInfo[] sourceFields = source.GetType().GetFields();

        foreach (var sourceField in sourceFields)
        {
            // Check if the field type is enum, numeric, boolean, or string
            if (IsSupportedFieldType(sourceField.FieldType) && sourceField.Name != "UID")
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