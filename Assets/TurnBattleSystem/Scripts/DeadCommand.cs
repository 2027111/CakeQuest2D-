
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadCommand : Command
{
    string message = " is Dead";
    public DeadCommand()
    {
    }
    public DeadCommand(string message)
    {
        this.message = message;
    }


    public override void ExecuteCommand()
    {
        Debug.Log("Lol");
        Source.StartCoroutine(Execute());
        Debug.Log(Source.name + message);
    }

    public virtual IEnumerator Execute()
    {

        yield return new WaitForSeconds(1.1f);
        OnExecuted?.Invoke();

    }


}