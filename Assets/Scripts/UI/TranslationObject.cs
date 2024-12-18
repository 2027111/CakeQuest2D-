using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TranslationObject : MonoBehaviour
{
    [SerializeField] SerializableDictionaries<string, List<TMP_Text>> TextObjects = new SerializableDictionaries<string, List<TMP_Text>>();

    private void Start()
    {
        ActualizeText();
        LanguageData.OnLanguageLoaded.AddListener(ActualizeText);
    }


    public void ActualizeText()
    {
        foreach (KeyValuePair<string, List<TMP_Text>> Texts in TextObjects)
        {
            if (Texts.Value != null)
            {
                if (Texts.Value.Count > 0)
                {
                    foreach (TMP_Text text in Texts.Value)
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
