using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class ConditionEvents
{
    public ConditionObject[] condition;
    public UnityEvent onConditionTrueEvent;
    public UnityEvent onConditionFalseEvent;



    public bool CheckCondition()
    {
        foreach (ConditionObject con in condition)
        {
            if (con != null)
            {
                if (!con.CheckCondition())
                {
                    return false;
                }
            }
        }
        return true;
    }
}
public class ConditionManager : MonoBehaviour
{
    public ConditionEvents[] conditionEvents;


    public void OnStart()
    {
        foreach (ConditionEvents ev in conditionEvents)
        {
            if (ev.CheckCondition())
            {
                ev.onConditionTrueEvent?.Invoke();
            }
            else
            {
                ev.onConditionFalseEvent?.Invoke();
            }
        }
    }

    private void Start()
    {
        OnStart();
    }
}
