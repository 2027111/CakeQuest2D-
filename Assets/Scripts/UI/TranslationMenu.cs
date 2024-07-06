using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TranslationMenu : MonoBehaviour
{
    public bool resetOnAwake = false;

    [SerializeField] SerializableDictionary<string, List<TMP_Text>> TextObjects = new SerializableDictionary<string, List<TMP_Text>>();

    private void Awake()
    {
        if (resetOnAwake)
        {
            Start();
        }
        GameSaveManager.Singleton?.OnLanguageChanged.AddListener(LoadLangue);
    }
    private void Start()
    {
        LanguageData.SetLanguage(GamePreference.Language);
        LoadLangue();
    }
    public void LoadLangue()
    {

        GamePreference.Language = LanguageData.GetLanguage();
        StartCoroutine(LanguageData.LoadJsonAsync(ActualizeText));
    }


    public void ActualizeText()
    {
        foreach(KeyValuePair<string, List<TMP_Text>> Texts in TextObjects)
        {
            if (Texts.Value != null)
            {
                if (Texts.Value.Count > 0)
                {
                    foreach(TMP_Text text in Texts.Value)
                    {
                        JsonData jsonData = LanguageData.GetDataById(LanguageData.MENUS);
                        string t = jsonData.GetValueByKey(Texts.Key);
                        text.text = t;
                    }
                }
            }
        }
    }


}


