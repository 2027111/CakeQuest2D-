using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestGiver : MonoBehaviour
{

    [SerializeField] QuestObject questObject;



    public void GiveQuest()
    {
        QuestManager.Singleton.GiveQuest(questObject);
    }
}
