using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;





[System.Serializable]
public class ConditionObject
{
    public BoolValue boolValue;
    public bool wantedResult = true;

    public bool CheckCondition()
    {
        if (!boolValue)
        {
            return true;
        }
        return boolValue.RuntimeValue == wantedResult;
    }
}
