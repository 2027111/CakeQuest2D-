using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestObject : BoolValue
{
    public string questName;
    public virtual void CheckConditions()
    {

    }

    public bool isCompleted()
    {
        return RuntimeValue;
    }

    public virtual void CompleteQuest()
    {
        SetRuntime();
    }
}
