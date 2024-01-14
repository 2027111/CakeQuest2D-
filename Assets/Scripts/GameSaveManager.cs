using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using TMPro;
using UnityEngine.Events;
using System;

public enum SaveFiles
{
    saveFile,
    translation
}



public class GameSaveManager : MonoBehaviour
{
    private static GameSaveManager _singleton;

    public List<ScriptableObject> objectsToSave = new List<ScriptableObject>();
    public List<ScriptableObject> objectsToTranslate = new List<ScriptableObject>();







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



    public LanguageObject currentLanguage;

    public string path;
    public string extension = "json";



    public UnityEvent OnLanguageChanged;
    public UnityEvent OnSaveFileLoaded;



    private void Awake()
    {
        Singleton = this;
        DontDestroyOnLoad(Singleton);
    }
    // Start is called before the first frame update
    void Start()
    {
        CreateSavePath();
        LoadSaveFile();
        LoadLanguage();
    }

    public void CreateSavePath()
    {

        InitializePath();
        foreach (var value in Enum.GetValues(typeof(SaveFiles)))
        {
            string enumValueName = value.ToString();

            // Check if the directory exists, if not, create it
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

    public void OnLanguageDropdownValueChanged(int value)
    {
        // Handle the dropdown value changed event
        // You can access the selected enum value using the dropdown's value property
        SetLanguage(value);
        OnChangeLanguage();
    }


    public void InitializePath()
    {

        path = $"{Application.persistentDataPath}";
    }

    public Language GetLanguage()
    {
        return currentLanguage.GetLanguage();
    }

    public void SetLanguage(int value)
    {
        currentLanguage.SetLanguage((Language)value);
    }

    private void OnDisable()
    {
    }



    public void OnChangeLanguage()
    {

        SaveGame();
        LoadLanguage();
    }



    public string GetPath(string whattosave)
    {
        return path + string.Format("/{0}", whattosave);
    }

    public string GetFilePath(string whattosave)
    {
        return path + string.Format("/{0}/{2}{0}.{1}", whattosave, extension, (whattosave == "translation" ? GetLanguage() + "_" : ""));
    }


    public void SaveGame()
    {
        SaveScriptables(SaveFiles.saveFile);
    }
    public void SaveDialogue()
    {
        SaveScriptables(SaveFiles.translation);
    }

    public void LoadLanguage()
    {

        LoadScriptables(SaveFiles.translation);

        OnLanguageChanged?.Invoke();
    }


    public void LoadSaveFile()
    {
        LoadScriptables(SaveFiles.saveFile);

        OnSaveFileLoaded?.Invoke();

    }

    public void SaveEverything()
    {



        SaveScriptables(SaveFiles.saveFile);
        SaveScriptables(SaveFiles.translation);
    }



    public void SaveScriptables(SaveFiles listName)
    {
        List<ScriptableObject> list;

        list = GetListMatch(listName);
        string filePath = GetFilePath(listName.ToString());
        List<ScriptableObjectDTO> dtoList = GetCurrentDTOList(list);
        var json = JsonUtility.ToJson(new ScriptableObjectListWrapper { objects = dtoList }, true);
        File.WriteAllText(filePath, json);
        
    }



    public void LoadScriptables(SaveFiles listName)
    {
        List<ScriptableObject> list;
            list = GetListMatch(listName);
            List<ScriptableObjectDTO> dtoList = ReadSaveFile(listName);
            if (dtoList != null)
            {
                ApplyToScriptableObjects(dtoList, list);
            }
        }
    public void ApplyToScriptableObjects(List<ScriptableObjectDTO> dtoList, List<ScriptableObject> list)
    {
        foreach (ScriptableObject obj in list)
        {
            ApplyToScriptableObjects(dtoList, obj);
        }
    }

    public void ApplyToScriptableObjects(List<ScriptableObjectDTO> dtoList, ScriptableObject obj)
    {
            ScriptableObject yourObj = obj;
            if (yourObj != null)
            {

                ScriptableObjectDTO scriptableObjectDTO = dtoList.Find(dto => dto.HashCode == yourObj.GetHashCode());


                // Find the corresponding DTO using the unique identifier
                if (scriptableObjectDTO != null)
                {
                    // Apply data to the ScriptableObject
                    scriptableObjectDTO.ApplyData(yourObj);
                }
            }
        
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




    private List<ScriptableObject> GetListMatch(SaveFiles listName)
    {
        switch (listName)
        {
            case SaveFiles.translation:
                return objectsToTranslate;
            case SaveFiles.saveFile:
                return objectsToSave;
            default: return null;
        }
    }
}