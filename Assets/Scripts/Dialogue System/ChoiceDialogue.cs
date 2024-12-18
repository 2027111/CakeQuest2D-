using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


using Unity.VisualScripting;

[Serializable, Inspectable]
public class ChoiceDialogue
{

    [Inspectable] public ConditionResultObject[] condition;
    [Inspectable] public string choicesLineIds;
    [Inspectable] public string[] dialogueLineIds;
    [Inspectable] public ChoiceDialogue[] choices;
    [Inspectable] public UnityEvent OnOverEvent;
    [Inspectable] public UnityEvent OnInstantOverEvent;
    public bool ConditionRespected()
    {
        foreach (ConditionResultObject c in condition)
        {
            if (!c.CheckCondition())
            {
                return false;
            }
        }
        return true;
    }

}


public enum BattleCondition
{
    None,
    OnLoop,
    OnTurn,
    OnEnemyTurn,
    OnObserveEnemy,
}
