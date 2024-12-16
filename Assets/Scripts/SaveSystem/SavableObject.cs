
using Newtonsoft.Json.Linq;
using System;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

public class SavableObject : ScriptableObject
{
    public string UID;

    private void OnValidate()
    {
#if UNITY_EDITOR
        if (string.IsNullOrEmpty(UID))
        {
            UID = GUID.Generate().ToString();
            EditorUtility.SetDirty(this);
        }
#endif
    }




    public virtual string GetJsonData()
    {
        var jsonObject = new JObject();
        jsonObject["SaveObjectName"] = name;
        jsonObject["UID"] = UID;


        return jsonObject.ToString();

    }


    public virtual void ApplyJsonData(string jsonData)
    {
        JsonUtility.FromJsonOverwrite(jsonData, this);
    }



    public bool Matches(string jsonObj)
    {
        try
        {
            // Parse the JSON string into a dynamic object
            JObject json = JObject.Parse(jsonObj);

            // Check if the JSON object contains the "UID" property
            if (json.ContainsKey("UID"))
            {
                // Get the UID from the JSON object and compare it with the current instance's UID
                string jsonUID = json["UID"].ToString();

                if (this.UID == jsonUID)
                {
                    return true;
                }
            }
        }
        catch (Exception ex)
        {
            // Handle any errors that occur during parsing or comparison
            Debug.LogError($"Error while parsing JSON: {ex.Message}");
        }

        return false;
    }


}