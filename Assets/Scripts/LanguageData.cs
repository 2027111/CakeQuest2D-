using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum Language
{
    Français,
    //English,
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
            return null;
        }



        foreach (var dataInfo in Singleton.Data)
        {
            if (dataInfo.dataId == id)
            {
                return dataInfo;
            }
        }
        return null;
    }
}



[Serializable]
public class JsonData
{
    public string dataId;
    public string jsonData;

    [Serializable]
    private class SerializableDictionary
    {
        public List<KeyValuePair<string, string>> keyValuePairs;
    }

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
                return value;
                
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







