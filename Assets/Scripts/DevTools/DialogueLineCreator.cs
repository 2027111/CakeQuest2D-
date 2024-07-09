using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;
using System;
using Newtonsoft.Json.Linq;

public class DialogueLineCreator : MonoBehaviour
{

    [SerializeField] TMP_Dropdown languageDropdown;
    [SerializeField] TMP_Dropdown IdsDropdown;
    [SerializeField] TMP_Dropdown portraitDropDown;
    [SerializeField] TMP_InputField lineField;
    [SerializeField] TMP_InputField nameField;
    [SerializeField] TMP_InputField idField;
    [SerializeField] TMP_InputField portraitField;
    [SerializeField] GameObject portraitContainer;
    [SerializeField] Image portraitImage;
    private List<string> allPortraitFilePaths = new List<string>();



    public Language language = Language.Français;



    LanguageData localData = new LanguageData();

    // Start is called before the first frame update
    void Start()
    {

        InitPortraitDropdown();
        InitLanguageDropdown();
        GetLocalData();

    }
    private void InitPortraitDropdown()
    {
        string portraitsFolderPath = Path.Combine(Application.dataPath, "Resources/Portraits");
        allPortraitFilePaths = GetAllFilePathsInFolder(portraitsFolderPath);
        for (int i = 0; i < allPortraitFilePaths.Count; i++)
        {
            string relativePath = allPortraitFilePaths[i].Replace(Path.Combine(Application.dataPath, "Resources/"), "");
            relativePath = relativePath.Replace("\\", "/");
            allPortraitFilePaths[i] = Path.ChangeExtension(relativePath, null);
        }
        allPortraitFilePaths.Add("...");
        // Clear existing options
        portraitDropDown.ClearOptions();

        // Get the names of the enum values

        // Convert the enum names to dropdown options

        List<TMP_Dropdown.OptionData> dropdownOptions = new List<TMP_Dropdown.OptionData>();
        for (int i = 0; i < allPortraitFilePaths.Count; i++)
        {
            dropdownOptions.Add(new TMP_Dropdown.OptionData(allPortraitFilePaths[i]));
        }

        // Add the options to the dropdown
        portraitDropDown.AddOptions(new System.Collections.Generic.List<TMP_Dropdown.OptionData>(dropdownOptions));
        portraitDropDown.onValueChanged.AddListener(UpdatePortraitValue);
        portraitDropDown.SetValueWithoutNotify(allPortraitFilePaths.Count - 1);
    }

    private void UpdatePortraitValue(int value)
    {

        portraitField?.SetTextWithoutNotify(allPortraitFilePaths[value]=="..."?"":allPortraitFilePaths[value]);
        portraitField?.onValueChanged.Invoke(portraitField.text);
    }

    private void InitLanguageDropdown()
    {
        // Clear existing options
        languageDropdown.ClearOptions();

        // Get the names of the enum values
        string[] languageNames = System.Enum.GetNames(typeof(Language));

        // Convert the enum names to dropdown options
        TMP_Dropdown.OptionData[] dropdownOptions = new TMP_Dropdown.OptionData[languageNames.Length];

        for (int i = 0; i < languageNames.Length; i++)
        {
            dropdownOptions[i] = new TMP_Dropdown.OptionData(languageNames[i]);
        }

        // Add the options to the dropdown
        languageDropdown.AddOptions(new System.Collections.Generic.List<TMP_Dropdown.OptionData>(dropdownOptions));
        languageDropdown.onValueChanged.AddListener(LanguageValueChange);
        LanguageValueChange(languageDropdown.value);
    }

    public void NewLine()
    {
        IdsDropdown.SetValueWithoutNotify(IdsDropdown.options.Count-1);
        IdsValueChanged(IdsDropdown.value);
    }

    public static List<string>GetAllFilePathsInFolder(string folderPath)
    {
        if (Directory.Exists(folderPath))
        {
            return new List<string>(Directory.GetFiles(folderPath, "*.png", SearchOption.AllDirectories));
        }
        else
        {
            Debug.LogError("Directory does not exist: " + folderPath);
            return new List<string>();
        }
    }

    private void InitIdsDropdown()
    {
        // Clear existing options
        int length = IdsDropdown.options.Count;
        int currentValue = IdsDropdown.value;
        IdsDropdown.ClearOptions();

        // Get the names of the enum values

        string[] languageNames = System.Enum.GetNames(typeof(Language));

        // Convert the enum names to dropdown options
        List<TMP_Dropdown.OptionData>dropdownOptions = new List<TMP_Dropdown.OptionData>();
        foreach (KeyValuePair<string, JsonData> jsonData in localData.translationData)
        {
                dropdownOptions.Add(new TMP_Dropdown.OptionData(jsonData.Key));
            
        }
        dropdownOptions.Add(new TMP_Dropdown.OptionData("..."));
        // Add the options to the dropdown
        IdsDropdown.AddOptions(new System.Collections.Generic.List<TMP_Dropdown.OptionData>(dropdownOptions));
        IdsDropdown.onValueChanged.AddListener(IdsValueChanged);
        int newValue = length == dropdownOptions.Count ? currentValue : dropdownOptions.Count - 1;


        if(dropdownOptions.Count == 1)
        {
            
            newValue = 0;
            NewLine();
            return;
        }
        IdsDropdown.SetValueWithoutNotify(newValue);
        IdsValueChanged(newValue);
    }


    public void IdsValueChanged(int value)
    {
        idField?.SetTextWithoutNotify(IdsDropdown.options[value].text);
        LoadLine();
    }
    public void LanguageValueChange(int value)
    {
        localData.SetLocalLanguage((Language)value);
        GetLocalData();
    }
    public void CreateLine()
    {
        // Collect values from input fields
        string portraitPath = portraitField?.text;
        string talkerName = nameField?.text;
        string line = lineField?.text;
        string id = idField?.text;
        if (string.IsNullOrEmpty(id.Trim()) || id.Contains("\"")){
            Debug.LogError($"Line could not be created because id is invalid");
            return;
        }


        id.Replace(" ", "_");
        idField?.SetTextWithoutNotify(id);
        // Construct the JSON string

        string jsonDataString = CreateJsonDataString(portraitPath, talkerName, line);

        // Create the dictionary with the required structure

        JsonData json = new JsonData(id, JsonDataType.Line ,jsonDataString);
        if (localData.translationData.TryGetValue(id, out JsonData value))
        {
            localData.translationData[id] = json;
        }
        else
        {
            localData.translationData.Add(id, json);
        }
        localData.Data.Add(json);

        InitIdsDropdown();

    }

    public string CreateJsonDataString(string portraitPath, string talkerName, string line)
    {
        var jsonObject = new JObject();

        if (!string.IsNullOrEmpty(portraitPath))
        {
            jsonObject["portraitPath"] = portraitPath;
        }

        if (!string.IsNullOrEmpty(talkerName))
        {
            jsonObject["talkerName"] = talkerName;
        }

        if (!string.IsNullOrEmpty(line))
        {
            jsonObject["line"] = line;
        }

        return jsonObject.ToString();
    }
    public IEnumerator SetPortrait(string portraitPath)
    {

        if (!string.IsNullOrEmpty(portraitPath) || portraitPath == "...")
        {
            if (allPortraitFilePaths.Contains(portraitPath))
            { // Load the sprite from Resources folder
                string fullPath = portraitPath; // Assuming the path is relative to the Resources folder
                ResourceRequest request = Resources.LoadAsync<Sprite>(fullPath);

                while (!request.isDone)
                {
                    yield return null;
                }

                Sprite portrait = request.asset as Sprite;

                if (portrait == null)
                {
                    // Log an error if the sprite failed to load
                    Debug.LogWarning("Failed to load sprite at path: " + fullPath);
                    // Optionally, list all loaded sprites for debugging
                    portraitContainer.gameObject.SetActive(false);

                }
                else
                {
                    portraitContainer.gameObject.SetActive(true);
                    // Assign the loaded sprite to the portrait image
                    portraitImage.sprite = portrait;
                }
                portraitDropDown.SetValueWithoutNotify(allPortraitFilePaths.IndexOf(portraitPath));
            }
            else
            {
                portraitContainer.gameObject.SetActive(false);

                portraitDropDown.SetValueWithoutNotify(allPortraitFilePaths.Count-1);
            }
           
        }
        else
        {

            portraitContainer.gameObject.SetActive(false);
        }

    }

    public void DeleteLine()
    {
        string id = idField?.text;
        if (localData.translationData.ContainsKey(id))
        {
            localData.translationData.Remove(id);
            NewLine();
        }
    }
    public void SetPortraitPath(string path)
    {
        StartCoroutine(SetPortrait(path));
    }
    public void LoadLine()
    {
        string id = idField?.text;
        if (localData.translationData.ContainsKey(id))
        {
            JsonData json = localData.GetLocalDataById(id);
            portraitField?.SetTextWithoutNotify(json.GetValueByKey("portraitPath"));
            portraitField?.onValueChanged.Invoke(portraitField.text);
            nameField?.SetTextWithoutNotify(json.GetValueByKey("talkerName"));
            lineField?.SetTextWithoutNotify(json.GetValueByKey("line"));
        }
        else
        {   
            if(idField?.text == "...")
            {
                idField?.SetTextWithoutNotify("");
            }
            portraitField?.SetTextWithoutNotify("");
            portraitField?.onValueChanged.Invoke(portraitField.text);
            nameField?.SetTextWithoutNotify("");
            lineField?.SetTextWithoutNotify("");
        }

    }

    public void GetLocalData()
    {
        string path = $"{Directory.GetCurrentDirectory()}/Assets/{(Language)languageDropdown.value}.json";
        localData = LanguageData.LoadLocalData(path, JsonDataType.Line);
        localData.SetGlobalDictionary();
        InitIdsDropdown();

    }
    public void ExportFile()
    {

        string path = $"{Directory.GetCurrentDirectory()}/Assets/{(Language)languageDropdown.value}";
        localData.SaveDataList(path);
    }
}
