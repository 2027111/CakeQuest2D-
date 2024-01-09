using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using TMPro;
using UnityEngine.Events;

public enum Language
{
    Français,
    English,
}

public class DialogueManager : MonoBehaviour
{
    private static DialogueManager _singleton;



    public List<ScriptableObject> objectsToTranslate;




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


    public void OnLanguageDropdownValueChanged(int value)
    {
        // Handle the dropdown value changed event
        // You can access the selected enum value using the dropdown's value property
        currentLanguage = (Language)value;
        OnChangeLanguage();
    }





    public Language currentLanguage = Language.Français;

    public string path;
    public string extension = "json";



    public UnityEvent OnLanguageChanged;






    private void Awake()
    {
        Singleton = this;
        DontDestroyOnLoad(Singleton);
    }
    // Start is called before the first frame update
    void Start()
    {
        path = $"{Application.persistentDataPath}/Dialogues";
        CreateSavePath();
    }

    private void CreateSavePath()
    {
        // Check if the directory exists, if not, create it
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
    }

    private void OnDisable()
    {
    }



    public void OnChangeLanguage()
    {
        LoadScriptables();
    }



    public string GetPath()
    {
        return path + string.Format("/{0}_dialogues.{1}", currentLanguage, extension);
    }


    public void SaveScriptables()
    {
        List<ScriptableObjectDTO> dtoList = new List<ScriptableObjectDTO>();

        foreach (ScriptableObject obj in objectsToTranslate)
        {
            // Create a ScriptableObjectDTO instance for serialization
            ScriptableObjectDTO scriptableObjectDTO = new ScriptableObjectDTO(obj);
            dtoList.Add(scriptableObjectDTO);
        }

        string filePath = GetPath(); // Use a specific name for the file

        // Serialize the entire List<ScriptableObjectDTO> using JsonUtility
        var json = JsonUtility.ToJson(new ScriptableObjectListWrapper { objects = dtoList });
        File.WriteAllText(filePath, json);
    }

    public void LoadScriptables()
    {
        string filePath = GetPath(); // Use the same specific name for the file

        if (File.Exists(filePath))
        {
            var json = File.ReadAllText(filePath);
            ScriptableObjectListWrapper listWrapper = JsonUtility.FromJson<ScriptableObjectListWrapper>(json);

            List<ScriptableObjectDTO> dtoList = listWrapper.objects;

            // Apply data to the ScriptableObjects
            for (int i = 0; i < Mathf.Min(dtoList.Count, objectsToTranslate.Count); i++)
            {
                ScriptableObject obj = objectsToTranslate[i];
                ScriptableObjectDTO scriptableObjectDTO = dtoList[i];
                scriptableObjectDTO.ApplyData(obj);

            }
        }

        OnLanguageChanged?.Invoke();
    }



}


