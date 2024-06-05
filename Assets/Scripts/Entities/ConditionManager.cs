using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class ConditionEvents
{
    public BoolValue condition;
    public UnityEvent onConditionTrueEvent;
    public UnityEvent onConditionFalseEvent;
}
public class ConditionManager : MonoBehaviour
{
    public ConditionEvents[] conditionEvents;



    private void Start()
    {
        foreach(ConditionEvents ev in conditionEvents)
        {
            if (ev.condition)
            {
                if (ev.condition.RuntimeValue)
                {
                    ev.onConditionTrueEvent?.Invoke();
                }
                else
                {
                    ev.onConditionFalseEvent?.Invoke();
                }
            }
        }
    }
}
