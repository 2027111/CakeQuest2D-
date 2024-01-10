using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TranslationMenu : MonoBehaviour
{
    [SerializeField] SerializableDictionary<string, List<TMP_Text>> TextObjects = new SerializableDictionary<string, List<TMP_Text>>();



    [SerializeField] MenuNames menuName;

    private void Start()
    {
        ActualizeText();
        DialogueManager.Singleton?.OnLanguageChanged.AddListener(ActualizeText);
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
                        text.text = menuName.GetStringValue(Texts.Key);
                    }
                }
            }
        }
    }


}


