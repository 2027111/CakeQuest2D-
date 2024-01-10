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



public class DialogueManager : MonoBehaviour
{
    private static DialogueManager _singleton;

    public List<YourScriptableObject> objectsToSave = new List<YourScriptableObject>();
    public List<YourScriptableObject> objectsToTranslate = new List<YourScriptableObject>();







    public static DialogueManager Singleton
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
                Debug.Log($"{nameof(DialogueManager)} instance already exists. Destroying duplicate!");
                Destroy(value.gameObject);
            }
        }
    }



    public LanguageObject currentLanguage;

    public string path;
    public string extension = "json";



    public UnityEvent OnLanguageChanged;
    public UnityEvent OnSaveFileLoaded;



    private void OnValidate()
    {

        path = $"{Application.persistentDataPath}";
    }

    public void OnLanguageDropdownValueChanged(int value)
    {
        // Handle the dropdown value changed event
        // You can access the selected enum value using the dropdown's value property
        SetLanguage(value);
        OnChangeLanguage();
    }

    public Language GetLanguage()
    {
        return currentLanguage.GetLanguage();
    }

    public void SetLanguage(int value)
    {
        currentLanguage.SetLanguage((Language)value);
    }

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


    private void OnDisable()
    {
    }



    public void OnChangeLanguage()
    {
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
        List<YourScriptableObject> list;

        list = GetListMatch(listName);
        string filePath = GetFilePath(listName.ToString());

            List<ScriptableObjectDTO> dtoList = new List<ScriptableObjectDTO>();

            foreach (YourScriptableObject obj in list)
            {
                ScriptableObjectDTO scriptableObjectDTO = new ScriptableObjectDTO(obj, obj.Id);
                dtoList.Add(scriptableObjectDTO);
            }

            var json = JsonUtility.ToJson(new ScriptableObjectListWrapper { objects = dtoList }, true);
            File.WriteAllText(filePath, json);
        
    }

    public void LoadScriptables(SaveFiles listName)
    {
        List<YourScriptableObject> list;
            list = GetListMatch(listName);
            string filePath = GetFilePath(listName.ToString());

            if (File.Exists(filePath))
            {
                var json = File.ReadAllText(filePath);
                ScriptableObjectListWrapper listWrapper = JsonUtility.FromJson<ScriptableObjectListWrapper>(json);

                List<ScriptableObjectDTO> dtoList = listWrapper.objects;

                foreach (YourScriptableObject obj in list)
            {
                Debug.Log(obj.Id.ToString());
                ScriptableObjectDTO scriptableObjectDTO = dtoList.Find(dto => dto.Id == ((YourScriptableObject)obj).Id.ToString());

                // Find the corresponding DTO using the unique identifier
                if (scriptableObjectDTO != null)
                    {
                        // Apply data to the ScriptableObject
                        scriptableObjectDTO.ApplyData(obj);
                    }
                }
            }
        }
    


    private List<YourScriptableObject> GetListMatch(SaveFiles listName)
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