using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu]
[System.Serializable]
public class BoolValue : SavableObject
{


    public bool RuntimeValue;

    public virtual void SetRuntime()
    {
        RuntimeValue = true;
    }
    public virtual void ResetRuntime()
    {
        RuntimeValue = false;
    }


    public override void ApplyData(SavableObject tempCopy)
    {

        RuntimeValue = (tempCopy as BoolValue).RuntimeValue;
        base.ApplyData(tempCopy);
    }

}
