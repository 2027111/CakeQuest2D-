using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

public enum Language
{
    Français,
    English
}

[Serializable]
public class LanguageData
{
    public static string INDICATION = "Indications";
    public static string CONTROLS = "ControlScheme";
    public static string MENUS = "MenuTranslations";



    public List<JsonData> Data = new List<JsonData>();
    public Dictionary<string, string> GlobalColors;
    public Dictionary<string, JsonData> translationData;
    public List<GlobalColor> globalColors = new List<GlobalColor>();
    public static Language language = Language.Français;
    public static Language defaultLanguage = Language.Français;
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

        if (Data != null && Data.Count > 0)
        {
            if (translationData == null)
            {
                translationData = new Dictionary<string, JsonData>();
            }

            foreach (var translation in Data)
            {
                translationData.Add(translation.dataId, translation);
            }

            // Optionally, you can log or debug the globalColors list
            // Debug.Log($"Global Colors converted: {JsonConvert.SerializeObject(GlobalColors)}");
        }
        else
        {
            if (translationData == null)
            {
                translationData = new Dictionary<string, JsonData>();
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
        return $"{language.ToString().ToLower()}";
    }

    private static LanguageData LoadGameData()
    {
        string languageSuffix = GetLanguageSuffix();



        TextAsset jsonFile = Resources.Load<TextAsset>($"translation/{languageSuffix}");


        if (jsonFile == null)
        {
            Debug.LogError($"Loaded asset is not a TextAsset for {languageSuffix}");
        }
        LanguageData combinedData = new LanguageData();

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


        return combinedData;
    }

    public static IEnumerator LoadAllJsonAsync(Action onComplete = null)
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

    public static IEnumerator LoadJsonAsync(Action onComplete = null)
    {
        string languageSuffix = GetLanguageSuffix();


        ResourceRequest request= Resources.LoadAsync<TextAsset>($"translation/{languageSuffix}");
        yield return request;

        if (request.asset == null)
        {
            Debug.LogError($"Failed to load translation file for {languageSuffix}");
            onComplete?.Invoke();
            yield break;
        }

        TextAsset jsonFile = request.asset as TextAsset;
        if (jsonFile == null)
        {
            Debug.LogError($"Loaded asset is not a TextAsset for {languageSuffix}");
            onComplete?.Invoke();
            yield break;
        }
        LanguageData combinedData = new LanguageData();

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

        
        Singleton = combinedData;
        onComplete?.Invoke();
    }



    public static JsonData GetDataById(string id)
    {
        if (Singleton == null)
        {
            return null;
        }

        if(Singleton.translationData.TryGetValue(id, out JsonData value))
        {
            return value;
        }
        Debug.LogWarning($"Key '{id}' not found in Translation File data.");
        return new JsonData(id, "");
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


    public JsonData(string id, string data)
    {
        this.dataId = id;
        this.jsonData = data;
    }
    public string GetValueByKey(string key)
    {
        if (string.IsNullOrEmpty(jsonData))
        {
            Debug.LogWarning("jsonData is null or empty.");
            return $"'{dataId} | {key}'";
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
                return $"No Line Found with Key '{dataId} | {key}'";
            }
            return $"'{dataId} | {key}'";
        }
        catch (Exception e)
        {
            Debug.LogError($"Error parsing JSON data: {e.Message}");
            return "";
        }
    }
}
