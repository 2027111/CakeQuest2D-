using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class GameSaveManager : MonoBehaviour
{
    private static GameSaveManager _singleton;



    public List<ScriptableObject> objectsToSave = new List<ScriptableObject>();
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
    private void Awake()
    {
        Singleton = this;
        DontDestroyOnLoad(Singleton);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }


    private void OnDisable()
    {
        SaveScriptables();
    }


    public void ResetScriptables()
    {
        for (int i = 0; i < objectsToSave.Count; i++)
        {
            if (File.Exists(Application.persistentDataPath + string.Format("/{0}.dat", i)))
            {
                File.Delete(Application.persistentDataPath + string.Format("/{0}.dat", i));
            }
        }
    }

    public void SaveScriptables()
    {
        for(int i = 0; i < objectsToSave.Count; i++)
        {
            FileStream file = File.Create(Application.persistentDataPath + string.Format("/{0}.dat", i));
            BinaryFormatter binary = new BinaryFormatter();
            var json = JsonUtility.ToJson(objectsToSave[i]);
            binary.Serialize(file, json);
            file.Close();
        }
    }

    public void LoadScriptables()
    {
        for (int i = 0; i < objectsToSave.Count; i++)
        {
            if(File.Exists(Application.persistentDataPath + string.Format("/{0}.dat", i)))
            {
                FileStream file = File.Open(Application.persistentDataPath + string.Format("/{0}.dat", i), FileMode.Open);
                BinaryFormatter binary = new BinaryFormatter();
                JsonUtility.FromJsonOverwrite((string)binary.Deserialize(file), objectsToSave[i]);
                file.Close();   
            }
        }
    }
    
}
