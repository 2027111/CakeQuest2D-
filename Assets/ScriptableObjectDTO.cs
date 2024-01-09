using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

[System.Serializable]
public class ScriptableObjectDTO
{
    public string typeName; // To store the type name
    public string jsonData; // To store the serialized JSON data

    public ScriptableObjectDTO(ScriptableObject scriptableObject)
    {
        typeName = scriptableObject.GetType().FullName;
        jsonData = JsonUtility.ToJson(scriptableObject);
    }

    public void ApplyData(ScriptableObject scriptableObject)
    {
        // Ensure the type matches before deserializing
        if (scriptableObject.GetType().FullName.Equals(typeName))
        {
            // Log JSON data

            // Create a temporary ScriptableObject copy
            ScriptableObject tempCopy = ScriptableObject.CreateInstance(scriptableObject.GetType());

            // Deserialize JSON data into the temporary copy
            JsonUtility.FromJsonOverwrite(jsonData, tempCopy);

            // Compare and apply only string fields from the temporary copy to the target ScriptableObject
            ApplyStringFields(scriptableObject, tempCopy);

            // Log information about the deserialization
        }
        else
        {
            Debug.LogError($"Type mismatch when deserializing ScriptableObject. Expected {typeName}, got {scriptableObject.GetType().FullName}");
        }
    }

    private void ApplyStringFields(ScriptableObject target, ScriptableObject source)
    {
        // Get all fields of the target and source objects
        System.Reflection.FieldInfo[] targetFields = target.GetType().GetFields();
        System.Reflection.FieldInfo[] sourceFields = source.GetType().GetFields();

        foreach (var sourceField in sourceFields)
        {
            // Check if the field is a string
            if (sourceField.FieldType == typeof(string))
            {
                // Find the corresponding field in the target object
                var matchingField = System.Array.Find(targetFields, field => field.Name == sourceField.Name);

                // Apply the string value from the source to the target
                if (matchingField != null)
                {
                    matchingField.SetValue(target, sourceField.GetValue(source));
                }
            }
        }
    }

}

[System.Serializable]
public class ScriptableObjectListWrapper
{
    public List<ScriptableObjectDTO> objects;
}
