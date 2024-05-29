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
    saveFile
}

public class GameSaveManager : MonoBehaviour
{
    private static GameSaveManager _singleton;

    public List<ScriptableObject> data = new List<ScriptableObject>();
    public List<ScriptableObject> playerData = new List<ScriptableObject>();

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
        return $"{path}/{whattosave}";
    }
    private string GetDefaultFilePath(string whattosave)
    {
        return $"{path}/save_default_{version}.{extension}";
    }


    public string GetFilePath(string whattosave)
    {
        return $"{path}/{whattosave}/save_{saveFileIndex}_{version}.{extension}";
    }

    public void SaveGame()
    {
        StartCoroutine(SaveFileCoroutine());
    }
    public IEnumerator SaveFileCoroutine()
    {
        yield return SaveScriptablesAsync(SaveFiles.saveFile);

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
        yield return StartCoroutine(LoadScriptablesCoroutine(SaveFiles.saveFile));
    }


    [ContextMenu("Test NewSoft")]
    public void ExportTemp()
    {
        var settings = new JsonSerializerSettings();

        var json = JsonConvert.SerializeObject(playerData);
        var saveTask = Task.Run(() => File.WriteAllTextAsync(Application.dataPath + "/temp.json", json));
        Debug.Log("Exported data to temp.json");
    }

    [ContextMenu("Test LoadNewsoft")]
    public void ImportTemp() { 

        var jdata = File.ReadAllText(Application.dataPath + "/temp.json");
        var pData = JsonConvert.DeserializeObject<List<ScriptableObject>>(jdata);
        Debug.Log("Imported data from temp.json");
        SetPlayerData(pData);
    }

    private void SetPlayerData(List<ScriptableObject> pData)
    {
        foreach(ScriptableObject obj in playerData)
        {

        }
    }

    public IEnumerator SaveScriptablesAsync(SaveFiles listName)
    {
        List<ScriptableObject> list = GetListMatch(listName);
        string filePath = GetFilePath(listName.ToString());
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
        List<ScriptableObject> list = GetListMatch(listName);
        List<ScriptableObjectDTO> dtoList = ReadDefaultSaveFile(listName);
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
            List<ScriptableObject> loadList = GetListMatch(listName);
            List<ScriptableObjectDTO> dtoloadList = ReadSaveFile(listName);
            if (dtoloadList != null)
            {
                yield return ApplyToScriptableObjects(dtoloadList, loadList);
            }
            yield return null;
        }
    }

    public void LoadScriptables(SaveFiles listName)
    {
        List<ScriptableObject> list = GetListMatch(listName);
        List<ScriptableObjectDTO> dtoList = ReadSaveFile(listName);
        if (dtoList != null)
        {
            ApplyToScriptableObjects(dtoList, list);
        }
    }

    public IEnumerator ApplyToScriptableObjects(List<ScriptableObjectDTO> dtoList, List<ScriptableObject> list)
    {
        foreach (ScriptableObject obj in list)
        {
           yield return ApplyToScriptableObjects(dtoList, obj);
            yield return null;
        }
    }

    public IEnumerator ApplyToScriptableObjects(List<ScriptableObjectDTO> dtoList, ScriptableObject obj)
    {
        ScriptableObject yourObj = obj;
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

    public ScriptableObjectDTO GetMatchingObject(List<ScriptableObjectDTO> dtoList, ScriptableObject obj)
    {
        foreach (ScriptableObjectDTO sodto in dtoList)
        {
            if (sodto.typeName == obj.GetType().Name)
            {
                if (sodto.objectName == obj.name || sodto.HashCode == obj.GetHashCode())
                {
                    return sodto;
                }
            }
        }
        return null;
    }

    public List<ScriptableObjectDTO> GetCurrentDTOList(List<ScriptableObject> list)
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
        string filePath = GetFilePath(path.ToString());
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


    private List<ScriptableObject> GetListMatch(SaveFiles listName)
    {
        switch (listName)
        {
            case SaveFiles.saveFile:
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
        string filePath = $"{path}/{SaveFiles.saveFile}/save_{index}_{version}.{extension}";
        if (File.Exists(filePath))
        {
            File.Delete(filePath);
        }
    }



    public int GetNumberOfSaveSlots()
    {
        string saveFilePath = GetPath(SaveFiles.saveFile.ToString()) + "/";
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
