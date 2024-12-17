using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


using Unity.VisualScripting;

[Serializable, Inspectable]
public class TestDialogue
{

    [Inspectable] public ConditionResultObject[] condition;
    [Inspectable] public string choicesLineIds;
    [Inspectable] public string[] dialogueLineIds;
    [Inspectable] public TestDialogue choices;
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


