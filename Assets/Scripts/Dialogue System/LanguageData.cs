using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public enum Language
{
    Français,
}

[Serializable]
public class LanguageData
{
    public List<JsonData> Data;
    public Dictionary<string, string> GlobalColors;
    public List<GlobalColor> globalColors;
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
                _singleton.SetGlobalDictionary();
            }
        }
    }

    public void SetGlobalDictionary()
    {
        if (globalColors != null && globalColors.Count > 0)
        {
            GlobalColors = new Dictionary<string,string>();

            foreach (var color in globalColors)
            {
                GlobalColors.Add(color.key, color.value);
            }

            // Optionally, you can log or debug the globalColors list
            //Debug.Log($"Global Colors converted: {JsonConvert.SerializeObject(GlobalColors)}");
        }
        else
        {
            Debug.LogWarning("GlobalColors is null or empty.");
            GlobalColors = new Dictionary<string, string>(); // Initialize an empty list to avoid null reference issues
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
            try
            {
                LanguageData data = JsonUtility.FromJson<LanguageData>(jsonFile.text);
                Debug.Log("GlobalColors Loaded: " + (data.GlobalColors != null ? JsonConvert.SerializeObject(data.GlobalColors) : "null"));
                return data;
            }
            catch (Exception e)
            {
                Debug.LogError($"Error parsing JSON data: {e.Message}");
            }
        }
        else
        {
            Debug.LogError("Failed to load gameData.json");
        }

        Resources.UnloadUnusedAssets();
        return null;
    }

    public static IEnumerator LoadJsonAsync(Action onComplete = null)
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
            try
            {
                LanguageData data = JsonUtility.FromJson<LanguageData>(jsonFile.text);
                Singleton = data;
                Debug.Log("GlobalColors Loaded Async: " + (Singleton.GlobalColors != null ? JsonConvert.SerializeObject(Singleton.GlobalColors) : "null"));
            }
            catch (Exception e)
            {
                Debug.LogError($"Error parsing JSON data: {e.Message}");
            }
        }
        else
        {
            Debug.LogError("Failed to load " + GetFilePath());
        }
        onComplete?.Invoke();
    }

    public static JsonData GetDataById(string id)
    {
        if (Singleton == null)
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
public class GlobalColor
{
    public string key;
    public string value;
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
            Debug.LogWarning("jsonData is null or empty.");
            return "E404";
        }

        try
        {
            // Deserialize the jsonData string into a SerializableDictionary
            Dictionary<string, string> values = JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonData);

            if (values.TryGetValue(key, out string value))
            {
                // Replace <sprite name=something> with <sprite name=something_else>
                string pattern = @"<sprite name=([\w\d_]+)>";
                string replacedText = Regex.Replace(value, pattern, match => {
                    string originalName = match.Groups[1].Value;
                    string newName = originalName + "_" + InputManager.controlSettings; // Example replacement
                    return $"<sprite name={newName}>";
                });

                // Replace {colorName} placeholders with corresponding values from GlobalColors
                pattern = @"\{color_([\w\d_]+)\}";
                replacedText = Regex.Replace(replacedText, pattern, match =>
                {
                    Debug.Log(match.Groups[1].Value);

                    string placeholder = match.Groups[1].Value;
                    if (LanguageData.Singleton.GlobalColors.TryGetValue(placeholder, out string colorValue))
                    {
                        return colorValue;
                    }
                    return match.Value;
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
