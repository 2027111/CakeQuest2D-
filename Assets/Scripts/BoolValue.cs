using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu]
[System.Serializable]
public class BoolValue : ScriptableObject
{


    public bool RuntimeValue;

    public virtual void SetRuntime()
    {
        RuntimeValue = true;
    }


}
