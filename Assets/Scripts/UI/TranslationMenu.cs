using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

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
        StartCoroutine(LoadAllText(delegate {
            LanguageData.SetLanguage(GamePreference.Language);
            LoadLangue();
        }));
        
    }


    public IEnumerator LoadAllText(Action callback)
    {

        TextObjects = new SerializableDictionary<string, List<TMP_Text>>();
        TMP_Text[] texts = GameObject.FindObjectsOfType<TMP_Text>();
        foreach (TMP_Text text in texts)
        {
            string content = text.text;
            JsonData translations = LanguageData.GetDataById(LanguageData.MENUS);
            if (translations.ContainsKey(content))
            {
                if (TextObjects.TryGetValue(content, out List<TMP_Text> textsObj))
                {
                    if (!textsObj.Contains(text))
                    {
                        textsObj.Add(text);
                    }
                }
                else
                {
                    TextObjects.Add(content, new List<TMP_Text> { text });
                }
            }
            yield return null;
        }
        callback?.Invoke();
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
                        if (jsonData.ContainsKey(Texts.Key))
                        {
                            string t = jsonData.GetValueByKey(Texts.Key);
                            text.text = t;
                        }
                    }
                }
            }
        }
    }


}


