using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TranslationMenu : MonoBehaviour
{
    [SerializeField] SerializableDictionary<string, List<TMP_Text>> TextObjects = new SerializableDictionary<string, List<TMP_Text>>();
    [SerializeField] string menuId = "MenuTranslations";

    private void Start()
    {
        LoadLangue();
        GameSaveManager.Singleton?.OnLanguageChanged.AddListener(LoadLangue);
    }
    public void LoadLangue()
    {
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
                        JsonData jsonData = LanguageData.GetDataById(menuId);
                        string t = jsonData.GetValueByKey(Texts.Key);
                        text.text = t;
                    }
                }
            }
        }
    }


}


