using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
public enum Language
{
    Français,
    English,
}



[Serializable]
public class LanguageData
{
    public List<JsonData> Data;
    public static Language language = Language.Français;

    
    private static LanguageData _singleton;
    public static LanguageData Singleton
    {
        get
        {
            
            return _singleton;
        }
        private set
        {
            if (value != null)
            {
                _singleton = value;
            }
        }
    }

    public static Language GetLanguage()
    {
        return language;
    }

    public static bool SetLanguage(Language value)
    {
        if (language != value)
        {
            language = value;
            return true;
        }
        return false;
    }


    public static string GetFilePath()
    {
        return "translation/" + language.ToString().ToLower();
    }
    private static LanguageData LoadGameData()
    {
    
        // Load the JSON text file from the Resources folder
        TextAsset jsonFile = Resources.Load<TextAsset>(GetFilePath()); // No need to include the .json extension
        if (jsonFile != null)
        {
            return JsonUtility.FromJson<LanguageData>(jsonFile.text);
        }
        else
        {
            Debug.LogError("Failed to load gameData.json");
        }

        return null;
    }

    public static IEnumerator LoadJsonAsync(System.Action onComplete = null)
    {
        string filePath = GetFilePath(); // No need to include the .json extension
        ResourceRequest request = Resources.LoadAsync<TextAsset>(filePath);
        while (!request.isDone)
        {
            yield return null;
        }

        TextAsset jsonFile = (TextAsset)request.asset;

        if (jsonFile != null)
        {
            LanguageData data = JsonUtility.FromJson<LanguageData>(jsonFile.text);
            Singleton = data;
        }
        else
        {
            Debug.LogError("Failed to load " + GetFilePath());
        }
        onComplete?.Invoke();
    }


    public static JsonData GetDataById(string id)
    {
        if(Singleton == null)
        {
            Singleton = LoadGameData();
            //return null;
        }



        foreach (var dataInfo in Singleton.Data)
        {
            if (dataInfo.dataId == id)
            {
                return dataInfo;
            }
        }
        return new JsonData();
    }
}



[Serializable]
public class JsonData
{
    public string dataId;
    public string jsonData;


    public string GetValueByKey(string key)
    {
        if (string.IsNullOrEmpty(jsonData))
        {
            Debug.LogError("jsonData is null or empty.");
            return "E404";
        }

        try
        {
            // Deserialize the jsonData string into a SerializableDictionary
            Dictionary<string, string> values = JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonData);

            if (values.TryGetValue(key, out string value))
            {


                string pattern = @"<sprite name=([\w\d_]+)>";

                // Replace <sprite name=something> with <sprite name=something_else>
                string replacedText = Regex.Replace(value, pattern, match => {
                    string originalName = match.Groups[1].Value;
                    // You can customize the replacement logic here
                    string newName = originalName + "_" + InputManager.controlSettings; // Example replacement
                    return $"<sprite name={newName}>";
                });



                return replacedText;
                
            }

            Debug.LogWarning($"Key '{key}' not found in JSON data.");
            return "E404";
        }
        catch (Exception e)
        {
            Debug.LogError($"Error parsing JSON data: {e.Message}");
            return "E404";
        }
    }
}







