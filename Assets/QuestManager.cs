using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

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

    public UnityEvent OnQuestCheck;

    [SerializeField] List<QuestObject> currentQuests = new List<QuestObject>();
    private void Awake()
    {
        Singleton = this;
        DontDestroyOnLoad(this.gameObject);
    }

    public List<QuestObject> GetQuests()
    {
        return currentQuests;
    }

    public void GiveQuest(QuestObject questObject)
    {
        if (!currentQuests.Contains(questObject))
        {
            currentQuests.Add(questObject);
        }

        UICanvas.UpdateQuestList();
        CheckQuests();
    }

    public void CheckQuests()
    {
        foreach(QuestObject questObject in currentQuests)
        {
            questObject.CheckConditions();

            if (questObject.RuntimeValue)
            {
                StartCoroutine(RemoveQuest(questObject));
            }
        }
        OnQuestCheck?.Invoke();
    }

    IEnumerator RemoveQuest(QuestObject questObject)
    {
        yield return new WaitForSeconds(2f);
        if (currentQuests.Contains(questObject))
        {
            currentQuests.Remove(questObject);
        }

        UICanvas.UpdateQuestList();
        CheckQuests();
    }
}
