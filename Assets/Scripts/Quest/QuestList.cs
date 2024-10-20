using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestList : MonoBehaviour
{
    [SerializeField] GameObject questIndicatorPrefab;
    [SerializeField] Transform container;
    public bool Affiched = false;


    public void CreateIndicator(QuestObject qobject)
    {
        GameObject t = Instantiate(questIndicatorPrefab, container);
        QuestIndicator qi = t.GetComponent<QuestIndicator>();
        qi.SetQuestObject(qobject);
    }

    public void ResetList()
    {
        foreach (Transform t in container)
        {
            Destroy(t.gameObject);
        }

        if (QuestManager.Singleton)
        {
            foreach (QuestObject q in QuestManager.Singleton.GetQuests())
            {
                if (!q.RuntimeValue && q.QuestToggled)
                {
                    CreateIndicator(q);
                }
            }
        }
        Appear(true);
    }

    public void Appear(bool on)
    {

        if (container.childCount > 0)
        {
            if (Affiched != on)
            {
                GetComponent<Animator>().SetTrigger(on ? "Appear" : "Disappear");
                Affiched = on;
            }
        }
        else
        {
            if (Affiched)
            {
                GetComponent<Animator>().SetTrigger("Disappear");
            }
        }
    }
}
