using Newtonsoft.Json.Linq;
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
        Debug.Log("Set Runtime");
        RuntimeValue = true;
    }
    public virtual void ResetRuntime()
    {
        RuntimeValue = false;
    }


    public override string GetJsonData()
    {

        var jsonObject = JObject.Parse(base.GetJsonData()); // Start with base class data


        jsonObject["RuntimeValue"] = RuntimeValue; // Adding additional data

        return jsonObject.ToString();



    }

}
