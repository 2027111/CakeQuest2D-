using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{




    private static QuestManager _singleton;
    public static QuestManager Singleton
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
                Debug.Log($"{nameof(QuestManager)} instance already exists. Destroying duplicate!");
                Destroy(value.gameObject);
            }
        }
    }



    [SerializeField] List<QuestObject> currentQuests = new List<QuestObject>();
    private void Awake()
    {
        Singleton = this;
        DontDestroyOnLoad(this.gameObject);
    }
    public void GiveQuest(QuestObject questObject)
    {
        if (!currentQuests.Contains(questObject))
        {
            currentQuests.Add(questObject);
        }
    }

    public void CheckQuests()
    {
        foreach(QuestObject questObject in currentQuests)
        {
            questObject.CheckConditions();
        }
    }
}
