using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;





using Unity.VisualScripting;

[Serializable, Inspectable]
public class ConditionResultObject
{
    [Inspectable] public BoolValue boolValue;
    [Inspectable] public bool wantedResult = true;

    public bool CheckCondition()
    {
        if (!boolValue)
        {
            return true;
        }
        return boolValue.RuntimeValue == wantedResult;
    }
}
