using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwapCommand : Command
{

    int targetIndex = 0;
    public SwapCommand() : base()
    {
    }


    public override void ExecuteCommand()
    {
        Source.StartCoroutine(Execute());
        Debug.Log(Source.name + " SWAPPED WITH ME");
    }

    public virtual IEnumerator Execute()
    {

        yield return new WaitForSeconds(.4f);
        OnExecuted?.Invoke();

    }

    public void SetTargetIndex(int actorsIndex)
    {
        targetIndex = actorsIndex;
    }
}
