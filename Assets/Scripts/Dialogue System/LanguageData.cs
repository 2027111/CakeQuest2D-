using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

public enum Language
{
    Fran�ais,
    English
}

[Serializable]
public class LanguageData
{
    public static string INDICATION = "Indications";
    public static string CONTROLS = "ControlScheme";
    public List<JsonData> Data = new List<JsonData>();
    public Dictionary<string, string> GlobalColors;
    public List<GlobalColor> globalColors = new List<GlobalColor>();
    public static Language language = Language.Fran�ais;
    public static Language defaultLanguage = Language.Fran�ais;
    private static LanguageData _singleton;
    public static LanguageData Singleton
    {
        get
        {
            if (_singleton == null)
            {
                Singleton = LoadGameData();
            }
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


    public LanguageData()
    {

    }
    public void SetGlobalDictionary()
    {
        if (globalColors != null && globalColors.Count > 0)
        {
            if(GlobalColors == null)
            {
                GlobalColors = new Dictionary<string, string>();
            }

            foreach (var color in globalColors)
            {
                GlobalColors.Add(color.key, color.value);
            }

            // Optionally, you can log or debug the globalColors list
            // Debug.Log($"Global Colors converted: {JsonConvert.SerializeObject(GlobalColors)}");
        }
        else
        {
            if (GlobalColors == null)
            {
                GlobalColors = new Dictionary<string, string>();
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

    public static string GetLanguageSuffix()
    {
        return $"_{language.ToString().ToLower()}";
    }

    private static LanguageData LoadGameData()
    {
        string languageSuffix = GetLanguageSuffix();


        TextAsset[] jsonFiles = Resources.LoadAll<TextAsset>("translation");


        LanguageData combinedData = new LanguageData();
        foreach (var jsonFile in jsonFiles)
        {
            if (jsonFile.name.EndsWith(languageSuffix))
            {


                LanguageData data = JsonUtility.FromJson<LanguageData>(jsonFile.text);
                if (data != null)
                {
                    if (data.Data != null)
                    {
                        combinedData.Data.AddRange(data.Data);
                    }
                    if (data.globalColors != null)
                    {
                        combinedData.globalColors.AddRange(data.globalColors);
                    }
                }

            }
        }

        return combinedData;
    }

    public static IEnumerator LoadJsonAsync(Action onComplete = null)
    {
        string languageSuffix = GetLanguageSuffix();


        TextAsset[] jsonFiles = Resources.LoadAll<TextAsset>("translation");


        LanguageData combinedData = new LanguageData();
        foreach (var jsonFile in jsonFiles)
        {
            if (jsonFile.name.EndsWith(languageSuffix))
            {
                

                    LanguageData data = JsonUtility.FromJson<LanguageData>(jsonFile.text);
                    if (data != null)
                    {
                        if (data.Data != null)
                        {
                            combinedData.Data.AddRange(data.Data);
                        }
                        if (data.globalColors != null)
                        {
                            combinedData.globalColors.AddRange(data.globalColors);
                        }
                    }
                
            }
            yield return null;
        }
        Singleton = combinedData;
        onComplete?.Invoke();
    }

    public static JsonData GetDataById(string id)
    {
        if (Singleton == null)
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

        Debug.LogWarning($"Key '{id}' not found in Translation File data.");
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
            return "";
        }

        try
        {
            // Deserialize the jsonData string into a SerializableDictionary
            Dictionary<string, string> values = JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonData);

            if (values.TryGetValue(key, out string value))
            {
                // Replace <sprite name=something> with <sprite name=something_else>
                string pattern = @"<sprite name=([\w\d_]+)>";
                string replacedText = Regex.Replace(value, pattern, match =>
                {
                    string originalName = match.Groups[1].Value;
                    string newName = originalName + "_" + InputManager.controlSettings; // Example replacement
                    return $"<sprite name={newName}>";
                });

                // Replace {colorName} placeholders with corresponding values from GlobalColors
                pattern = @"\{color_([\w\d_]+)\}";
                replacedText = Regex.Replace(replacedText, pattern, match =>
                {
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
            if (key == "line")
            {
                return $"No Line Found with Key '{key}'";
            }
            return "";
        }
        catch (Exception e)
        {
            Debug.LogError($"Error parsing JSON data: {e.Message}");
            return "";
        }
    }
}
