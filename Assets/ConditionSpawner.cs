using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;





[System.Serializable]
public class Condition
{
    public BoolValue condition;
    public bool wantedResult = true;

    public bool CheckCondition()
    {
        if (!condition)
        {
            return true;
        }
        return condition.RuntimeValue == wantedResult;
    }
}
