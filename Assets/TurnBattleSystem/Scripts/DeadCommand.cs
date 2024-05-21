
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadCommand : Command
{
    public DeadCommand()
    {
    }


    public override void ExecuteCommand()
    {
        Source.StartCoroutine(Execute());
        Debug.Log(Source.name + " is Dead");
    }

    public virtual IEnumerator Execute()
    {

        yield return new WaitForSeconds(.4f);
        OnExecuted?.Invoke();

    }


}