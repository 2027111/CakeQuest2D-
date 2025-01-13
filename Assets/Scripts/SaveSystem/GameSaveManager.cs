using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

public enum SaveFiles
{
    save
}


[Serializable]
public class JSONSaveDataWrapper
{
    public List<string> AllInventories = new List<string>();
    public List<string> AllCharacterObjects = new List<string>();
    public List<string> AllParties = new List<string>();
    public List<string> Data = new List<string>();
    public float playTime = 0;

    public int saveIndex;
    public string GetAttribute(string attributeName)
    {
        foreach (var jsonStr in Data)
        {
            try
            {
                // Parse the JSON string into a JObject
                JObject jsonObj = JObject.Parse(jsonStr);

                // Check if the object contains the given attribute name
                if (jsonObj.ContainsKey(attributeName))
                {
                    // Return the value of the attribute
                    return jsonObj[attributeName].ToString();
                }
            }
            catch (Exception ex)
            {
                // Handle any errors that occur during parsing (e.g., malformed JSON)
                Debug.LogError($"Error parsing JSON: {ex.Message}");
            }
        }

        // Return null if the attribute was not found
        return null;
    }


    public bool GetRuntimeOfIdObject(string id)
    {
        foreach (var jsonStr in Data)
        {
            try
            {
                // Parse the JSON string into a JObject
                JObject jsonObj = JObject.Parse(jsonStr);

                // Check if the object contains the given attribute name
                if (jsonObj.ContainsKey("UID"))
                {
                    // Return the value of the attribute
                    if (jsonObj["UID"].ToString() == id)
                    {
                        if (jsonObj.ContainsKey("RuntimeValue"))
                        {
                            if (Boolean.TryParse(jsonObj["RuntimeValue"]?.ToString(), out bool value))
                            {
                                return value;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle any errors that occur during parsing (e.g., malformed JSON)
                Debug.LogError($"Error parsing JSON: {ex.Message}");
            }
        }

        // Return null if the attribute was not found
        return false;
    }

    public string GetAttributeOfIdObject(string id, string attributeName)
    {
        foreach (var jsonStr in Data)
        {
            try
            {
                // Parse the JSON string into a JObject
                JObject jsonObj = JObject.Parse(jsonStr);
                // Check if the object contains the given attribute name
                if (jsonObj.ContainsKey("UID"))
                {
                    // Return the value of the attribute
                    if (jsonObj["UID"].ToString() == id)
                    {


                        if (jsonObj.ContainsKey(attributeName))
                        {
                            return jsonObj[attributeName]?.ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle any errors that occur during parsing (e.g., malformed JSON)
                Debug.LogError($"Error parsing JSON: {ex.Message}");
            }
        }

        // Return null if the attribute was not found
        return null;
    }

}

public class GameSaveManager : MonoBehaviour
{
    private static GameSaveManager _singleton;
    JSONSaveDataWrapper currentLoadedData;
    public DateTime dateTime;


    [Header("Read Save Files")]
    public List<JSONSaveDataWrapper> saves = new List<JSONSaveDataWrapper>();

    [Header("Objects To Save")]
    public List<SavableObject> data = new List<SavableObject>();
    public List<CharacterInventory> allInventories = new List<CharacterInventory>();
    public List<CharacterObject> allCharacterObjects = new List<CharacterObject>();
    public List<Party> allParties = new List<Party>();


    public static int saveFileIndex = 0; // Default to the first save slot
    public string version = "0.0.05.28";

    public static GameSaveManager Singleton
    {
        get => _singleton;
        private set
        {
            if (_singleton == null)
            {
                _singleton = value;
            }
            else if (_singleton != value)
            {
                Debug.Log($"{nameof(GameSaveManager)} instance already exists. Destroying duplicate!");
                Destroy(value.gameObject);
            }
        }
    }


    public string path;
    public string extension = "json";

    public UnityEvent OnLanguageChanged;
    public UnityEvent OnSaveFileLoaded;
    public UnityEvent OnGamePreferenceLoaded;

    private void Awake()
    {
        Singleton = this;
        DontDestroyOnLoad(Singleton);
        InitializePath();
        GamePreference.SetPath($"{path}/preferences.json");
        GamePreference.LoadFromFile();
        CreateSavePath();
    }




    public void CreateSavePath()
    {
        foreach (var value in Enum.GetValues(typeof(SaveFiles)))
        {

            string enumValueName = value.ToString();
            if (!Directory.Exists(GetPath(enumValueName)))
            {
                Directory.CreateDirectory(GetPath(enumValueName));
            }

        }
    }

    private void OnValidate()
    {
        InitializePath();
    }

    public void InitializePath()
    {
        path = $"{Directory.GetCurrentDirectory()}";
    }

    public void OnLanguageDropdownValueChanged(int value)
    {
        SetLanguage(value);
        OnChangeLanguage();
    }

    public Language GetLanguage()
    {
        return LanguageData.GetLanguage();
    }

    public void SetLanguage(int value)
    {
        LanguageData.SetLanguage((Language)value);
    }

    public void OnChangeLanguage()
    {
        LoadLanguage();
    }

    public string GetPath(string whattosave)
    {
        return $"{path}/saveFile";
    }
    private string GetDefaultFilePath(string whattosave)
    {
        return $"{path}/{whattosave}_default_{version}.{extension}";
    }



    public string GetNewFilePath(string whattosave)
    {
        return $"{path}/saveFile/{whattosave}_{saveFileIndex}_{version}.{extension}";
    }


    public void SaveGame()
    {
        StartCoroutine(SaveFileCoroutine());
    }
    public IEnumerator SaveFileCoroutine()
    {
        yield return SaveScriptablesAsync(SaveFiles.save);

    }
    public void LoadLanguage()
    {
        OnLanguageChanged?.Invoke();
    }

    public IEnumerator LoadSaveFileCoroutine()
    {
        yield return StartCoroutine(LoadDefaultFileCoroutine(SaveFiles.save));
        yield return StartCoroutine(LoadSavableObjects());
    }



    public IEnumerator LoadDefaultFileCoroutine(SaveFiles listName)
    {
        JSONSaveDataWrapper listWrapper = ReadDefaultSaveFile(listName);


        yield return ImportStoredData(listWrapper);
    }




    public IEnumerator ImportStoredData(JSONSaveDataWrapper jSONSaveDataWrapper)
    {
        var settings = new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.All,
            NullValueHandling = NullValueHandling.Include
        };

        if (jSONSaveDataWrapper != null)
        {

            // Set the lists from the deserialized data
            SetPlayerData<CharacterInventory>((jSONSaveDataWrapper.AllInventories), allInventories);
            SetPlayerData<CharacterObject>((jSONSaveDataWrapper.AllCharacterObjects), allCharacterObjects);
            SetPlayerData<Party>((jSONSaveDataWrapper.AllParties), allParties);
            SetPlayerData<SavableObject>((jSONSaveDataWrapper.Data), data);
            yield return null;
        }
        else
        {
            Debug.LogError("Failed to deserialize data from save Data is null.");
        }

    }


    private IEnumerator LoadSavableObjects()
    {

        if (GetNumberOfSaveFiles() <= saveFileIndex)
        {
            saveFileIndex = GetNumberOfSaveFiles();
            currentLoadedData = null;
            //yield return SaveFileCoroutine();
        }
        else
        {

            JSONSaveDataWrapper saveData = saves[saveFileIndex];
            yield return ImportStoredData(saveData);

            foreach (SavableObject obj in data)
            {
                foreach (string jsonSaveObj in saveData.Data)
                {
                    if (obj.Matches(jsonSaveObj))
                    {

                        obj.ApplyJsonData(jsonSaveObj);
                    }
                }
            }
            yield return null;
        }
    }




    private void SetPlayerData<T>(List<string> loadedDataFiles, List<T> fileToApplyTo)
    {
        foreach (T obj in fileToApplyTo)
        {
            foreach (string data in loadedDataFiles)
            {

                if ((obj as SavableObject).Matches(data))
                {
                    (obj as SavableObject).ApplyJsonData(data);
                }

            }
        }
    }


    public IEnumerator SaveScriptablesAsync(SaveFiles listName)
    {
        var settings = new JsonSerializerSettings
        {
            Formatting = Formatting.Indented,
            NullValueHandling = NullValueHandling.Ignore
        };

        // Create dictionaries to store JSON data for each category
        // Create dictionaries to store JSON data for each category
        var saveDataDict = new Dictionary<string, List<string>>
            {
                { "AllInventories", allInventories.Select(obj => CleanJsonData(obj.GetJsonData())).ToList() },
                { "AllCharacterObjects", allCharacterObjects.Select(obj => CleanJsonData(obj.GetJsonData())).ToList() },
                { "AllParties", allParties.Select(obj => CleanJsonData(obj.GetJsonData())).ToList() },
                { "Data", GetListMatch(listName).Select(obj => CleanJsonData(obj.GetJsonData())).ToList() }
            };


        // Convert the dictionary to JSON format
        string saveDataJson = JsonConvert.SerializeObject(saveDataDict, settings);

        // Define file path
        string filePath = GetNewFilePath(listName.ToString());

        // Save JSON data to file asynchronously
        var saveTask = Task.Run(() => File.WriteAllTextAsync(filePath, saveDataJson));

        // Wait for the task to complete
        while (!saveTask.IsCompleted)
        {
            yield return null;
        }

        if (saveTask.Exception != null)
        {
            Debug.LogError($"Error saving file: {saveTask.Exception}");
        }
    }

    // Helper method to clean JSON data
    private string CleanJsonData(string jsonData)
    {
        // Trim leading/trailing whitespace
        jsonData = jsonData.Trim();

        // Replace carriage return/newline characters
        jsonData = jsonData.Replace("\r\n", "").Replace("\n", "");

        // Optionally, replace extra spaces with a single space if necessary
        // jsonData = jsonData.Replace("  ", " ");  // Use this if you want to collapse multiple spaces

        return jsonData;
    }


    public JSONSaveDataWrapper ReadSaveFile(SaveFiles path)
    {
        string filePath = GetNewFilePath(path.ToString());
        if (File.Exists(filePath))
        {
            var json = File.ReadAllText(filePath);
            JSONSaveDataWrapper listWrapper = JsonUtility.FromJson<JSONSaveDataWrapper>(json);
            return listWrapper;
        }
        return null;
    }





    public JSONSaveDataWrapper ReadDefaultSaveFile(SaveFiles path)
    {


        string defaultPath = "default_save_file";



        TextAsset jsonFile = Resources.Load<TextAsset>($"{defaultPath}");

        if(jsonFile != null)
        {
            Debug.Log("Loaded Default File");
            JSONSaveDataWrapper listWrapper = JsonUtility.FromJson<JSONSaveDataWrapper>(jsonFile.text);
            return listWrapper;
        }
        return null;
    }


    private List<SavableObject> GetListMatch(SaveFiles listName)
    {
        switch (listName)
        {
            case SaveFiles.save:
                return data;
            default:
                return null;
        }
    }

    // Methods to manage save slots
    public void SetSaveFileIndex(int index)
    {
        saveFileIndex = index;
    }

    public void CreateNewSaveSlot()
    {
        // Determine the next available save slot
        saveFileIndex = GetNumberOfSaveSlots();
    }

    public void DeleteSaveSlot(int index)
    {
        string filePath = $"{path}/{SaveFiles.save}/save_{index}_{version}.{extension}";
        if (File.Exists(filePath))
        {
            File.Delete(filePath);
        }
    }



    public int GetNumberOfSaveFiles()
    {
        string saveFilePath = GetPath(SaveFiles.save.ToString());
        if (Directory.Exists(saveFilePath))
        {
            List<string> files = new List<string>(Directory.GetFiles(saveFilePath, $"save_*_{version}.{extension}"));
            foreach (string f in files)
            {
                if (f.Split("_")[0].Contains("default"))
                {
                    files.Remove(f);
                }
            }
            return files.Count;
        }
        return -1;
    }
    public int GetNumberOfSaveSlots()
    {
        return saves.Count;
    }

    public void StartLoadingFiles(Action callback)
    {
        saves.Clear();
        StartCoroutine(LoadAllSaveFilesCoroutine(files =>
        {
            foreach (var file in files)
            {
                saves.Add(file);
            }

            callback();
        }));
    }
    public IEnumerator LoadAllSaveFilesCoroutine(System.Action<List<JSONSaveDataWrapper>> onComplete)
    {
        string saveFilePath = GetPath(SaveFiles.save.ToString());
        List<JSONSaveDataWrapper> saveDataList = new List<JSONSaveDataWrapper>();

        if (Directory.Exists(saveFilePath))
        {
            List<string> files = new List<string>(Directory.GetFiles(saveFilePath, $"save_*_{version}.{extension}"));

            // Remove "default" files
            files.RemoveAll(f => Path.GetFileName(f).Split('_')[0].Contains("default"));

            // Iterate through each file
            foreach (var file in files)
            {
                JSONSaveDataWrapper jsonSaveData = null;

                // Read the file asynchronously
                yield return StartCoroutine(ReadFileCoroutine(file, result => jsonSaveData = JsonUtility.FromJson<JSONSaveDataWrapper>(result)));

                if (jsonSaveData != null)
                {
                    // Simply add to the list of JSONSaveDataWrapper
                    saveDataList.Add(jsonSaveData);
                }

                // Simulate asynchronous operation
                yield return null;
            }

            // Invoke the callback with the loaded save data
            onComplete?.Invoke(saveDataList);
        }
        else
        {
            // If directory does not exist, invoke with an empty list
            onComplete?.Invoke(saveDataList);
        }
    }

    private IEnumerator ReadFileCoroutine(string filePath, System.Action<string> onComplete)
    {
        string fileContent = null;

        // Simulate asynchronous operation
        yield return new WaitForSeconds(0.1f); // Adjust delay as needed

        if (File.Exists(filePath))
        {
            fileContent = File.ReadAllText(filePath);
        }

        onComplete?.Invoke(fileContent);
    }
}
