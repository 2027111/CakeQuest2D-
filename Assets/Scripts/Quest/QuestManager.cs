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
                value.currentQuests = _singleton.currentQuests;
                Debug.Log($"{nameof(QuestManager)} instance already exists. Destroying duplicate!");
                Destroy(_singleton.gameObject);
                _singleton = value;
            }
        }
    }

    public UnityEvent OnQuestCheck;

    [SerializeField] List<QuestObject> currentQuests = new List<QuestObject>();
    private void Awake()
    {
        Singleton = this;
        DontDestroyOnLoad(this.gameObject);
        CheckQuests();
        UICanvas.UpdateQuestList();
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

        questObject.ToggleQuest();
        UICanvas.UpdateQuestList();
        CheckQuests();
    }

    public void RemoveQuest(QuestObject questObject)
    {
        questObject.ToggleQuest(false);
        UICanvas.UpdateQuestList();
        CheckQuests();
    }
    public static void CheckToggledQuests()
    {
        Singleton?.CheckQuests();
    }
    public void CheckQuests()
    {
        foreach(QuestObject questObject in currentQuests)
        {
            if (questObject.QuestToggled)
            {

            questObject.CheckConditions();

            if (questObject.RuntimeValue)
            {

                RemoveQuest(questObject);
                StartCoroutine(LateUpdateQuest());
            }
            }
        }
        OnQuestCheck?.Invoke();
    }

    IEnumerator LateUpdateQuest()
    {
        yield return new WaitForSeconds(2f);
        UICanvas.UpdateQuestList();
    }
}
