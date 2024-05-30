using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

public enum SaveFiles
{
    save
}

public class GameSaveManager : MonoBehaviour
{
    private static GameSaveManager _singleton;

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

    private void Awake()
    {
        Singleton = this;
        DontDestroyOnLoad(Singleton);
    }

    void Start()
    {
        CreateSavePath();
    }
    


    public void CreateSavePath()
    {
        InitializePath();
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
        path = $"{Application.dataPath}";
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
        SaveGame();
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
        yield return ExportStoredData();

    }
    public void LoadLanguage()
    {
        OnLanguageChanged?.Invoke();
    }

    public void LoadSaveFile()
    {
        StartCoroutine(LoadSaveFileCoroutine());
    }

    public IEnumerator LoadSaveFileCoroutine()
    {
        yield return StartCoroutine(LoadScriptablesCoroutine(SaveFiles.save));
    }


    public IEnumerator ExportStoredData()
    {
        var settings = new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.All,
            Formatting = Formatting.Indented,
            NullValueHandling = NullValueHandling.Ignore
        };

        // Create an instance of SaveDataWrapper and populate it
        var saveData = new SaveDataWrapper
        {
            AllInventories = allInventories,
            AllCharacterObjects = allCharacterObjects,
            AllParties = allParties
        };

        var json = JsonConvert.SerializeObject(saveData, settings);
        var saveTask = Task.Run(() => File.WriteAllTextAsync(GetNewFilePath("playerData"), json));
        while (!saveTask.IsCompleted)
        {
            yield return null;
        }
        yield return null;
    }


    public IEnumerator ImportStoredData()
    {
        var settings = new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.All,
            NullValueHandling = NullValueHandling.Include
        };

        if (File.Exists(GetNewFilePath("playerData")))
        {
            var jdata = File.ReadAllText(GetNewFilePath("playerData"));
            var saveData = JsonConvert.DeserializeObject<SaveDataWrapper>(jdata, settings);

            if (saveData != null)
            {
                Debug.Log("Imported data from allData.json");

                // Set the lists from the deserialized data
                SetPlayerData<CharacterInventory>((saveData.AllInventories), allInventories);
                SetPlayerData<CharacterObject>((saveData.AllCharacterObjects), allCharacterObjects);
                SetPlayerData<Party>((saveData.AllParties), allParties);
                yield return null;
            }
            else
            {
                Debug.LogError("Failed to deserialize data from allData.json. Data is null.");
            }
        }
        else
        {
            Debug.LogError("File allData.json does not exist.");
        }

    }


    public IEnumerator ImportDefaultStoredData()
    {
        var settings = new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.All,
            NullValueHandling = NullValueHandling.Include
        };

        if (File.Exists(GetDefaultFilePath("playerData")))
        {
            var jdata = File.ReadAllText(GetDefaultFilePath("playerData"));
            var saveData = JsonConvert.DeserializeObject<SaveDataWrapper>(jdata, settings);

            if (saveData != null)
            {
                Debug.Log("Imported data from allData.json");

                // Set the lists from the deserialized data
                SetPlayerData<CharacterInventory>((saveData.AllInventories), allInventories);
                SetPlayerData<CharacterObject>((saveData.AllCharacterObjects), allCharacterObjects);
                SetPlayerData<Party>((saveData.AllParties), allParties);
                yield return null;
            }
            else
            {
                Debug.LogError("Failed to deserialize data from allData.json. Data is null.");
            }
        }
        else
        {
            Debug.LogError("File allData.json does not exist.");
        }

    }





    private void SetPlayerData<T>(List<T> loadedDataFiles, List<T> fileToApplyTo)
    {
        foreach(T obj in fileToApplyTo)
        {
            foreach (T data in loadedDataFiles) 
            {

                if ((obj as SavableObject).UID.Equals((data as SavableObject).UID))
                {
                    (obj as SavableObject).ApplyData(data as SavableObject);
                }

            }
        }
    }

    

    public IEnumerator SaveScriptablesAsync(SaveFiles listName)
    {
        List<SavableObject> list = GetListMatch(listName);
        string filePath = GetNewFilePath(listName.ToString());
        List<ScriptableObjectDTO> dtoList = GetCurrentDTOList(list);
        var json = JsonUtility.ToJson(new ScriptableObjectListWrapper { objects = dtoList }, true);

        var saveTask = Task.Run(() => File.WriteAllTextAsync(filePath, json));

        while (!saveTask.IsCompleted)
        {
            yield return null;
        }

        if (saveTask.Exception != null)
        {
            Debug.LogError($"Error saving file: {saveTask.Exception}");
        }
    }

    private IEnumerator LoadScriptablesCoroutine(SaveFiles listName)
    {
        List<SavableObject> list = GetListMatch(listName);
        List<ScriptableObjectDTO> dtoList = ReadDefaultSaveFile(listName);
        yield return ImportDefaultStoredData();
        if (dtoList != null)
        {
            yield return ApplyToScriptableObjects(dtoList, list);
        }
        yield return null;
        if (GetNumberOfSaveSlots() == saveFileIndex)
        {
            
            yield return SaveFileCoroutine();
        }
        else
        {
            List<SavableObject> loadList = GetListMatch(listName);
            List<ScriptableObjectDTO> dtoloadList = ReadSaveFile(listName);

            if (dtoloadList != null)
            {
                yield return ApplyToScriptableObjects(dtoloadList, loadList);
            }
            yield return ImportStoredData();
            yield return null;
        }
    }

    public void LoadScriptables(SaveFiles listName)
    {
        List<SavableObject> list = GetListMatch(listName);
        List<ScriptableObjectDTO> dtoList = ReadSaveFile(listName);
        if (dtoList != null)
        {
            ApplyToScriptableObjects(dtoList, list);
        }
    }

    public IEnumerator ApplyToScriptableObjects(List<ScriptableObjectDTO> dtoList, List<SavableObject> list)
    {
        foreach (SavableObject obj in list)
        {
           yield return ApplyToScriptableObjects(dtoList, obj);
            yield return null;
        }
    }

    public IEnumerator ApplyToScriptableObjects(List<ScriptableObjectDTO> dtoList, SavableObject obj)
    {
        SavableObject yourObj = obj;
        if (yourObj != null)
        {
            ScriptableObjectDTO scriptableObjectDTO = GetMatchingObject(dtoList, obj);
            if (scriptableObjectDTO != null)
            {
                scriptableObjectDTO.ApplyData(yourObj);
            }
        }
        yield return null;
    }

    public ScriptableObjectDTO GetMatchingObject(List<ScriptableObjectDTO> dtoList, SavableObject obj)
    {
        foreach (ScriptableObjectDTO sodto in dtoList)
        {
            if (sodto.typeName == obj.GetType().Name)
            {
                if (sodto.objectName == obj.name || sodto.uid == obj.UID)
                {
                    return sodto;
                }
            }
        }
        return null;
    }

    public List<ScriptableObjectDTO> GetCurrentDTOList(List<SavableObject> list)
    {
        List<ScriptableObjectDTO> dtoList = new List<ScriptableObjectDTO>();
        for (int i = 0; i < list.Count; i++)
        {
            ScriptableObjectDTO scriptableObjectDTO = new ScriptableObjectDTO(list[i]);
            dtoList.Add(scriptableObjectDTO);
        }
        return dtoList;
    }

    public List<ScriptableObjectDTO> ReadSaveFile(SaveFiles path)
    {
        string filePath = GetNewFilePath(path.ToString());
        if (File.Exists(filePath))
        {
            var json = File.ReadAllText(filePath);
            ScriptableObjectListWrapper listWrapper = JsonUtility.FromJson<ScriptableObjectListWrapper>(json);
            List<ScriptableObjectDTO> dtoList = listWrapper.objects;
            return dtoList;
        }
        return null;
    }

    public List<ScriptableObjectDTO> ReadDefaultSaveFile(SaveFiles path)
    {
        string filePath = GetDefaultFilePath(path.ToString());
        if (File.Exists(filePath))
        {
            var json = File.ReadAllText(filePath);
            ScriptableObjectListWrapper listWrapper = JsonUtility.FromJson<ScriptableObjectListWrapper>(json);
            List<ScriptableObjectDTO> dtoList = listWrapper.objects;
            return dtoList;
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



    public int GetNumberOfSaveSlots()
    {
        string saveFilePath = GetPath(SaveFiles.save.ToString());
        if (Directory.Exists(saveFilePath))
        {
            List<string> files = new List<string>(Directory.GetFiles(saveFilePath, $"save_*_{version}.{extension}"));
            foreach(string f in files)
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
}
