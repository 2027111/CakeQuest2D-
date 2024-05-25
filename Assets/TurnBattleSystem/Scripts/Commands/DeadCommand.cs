
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadCommand : Command
{
    string message = " is Dead";
    public DeadCommand()
    {
        Target = new List<BattleCharacter>();
    }
    public DeadCommand(string message) : base()
    {
        this.message = message;
    }


    public override void ExecuteCommand()
    {
        Source.StartCoroutine(Execute());
        Debug.Log(Source.name + message);
    }

    public virtual IEnumerator Execute()
    {

        yield return new WaitForSeconds(.3f);
        OnExecuted?.Invoke();

    }


}