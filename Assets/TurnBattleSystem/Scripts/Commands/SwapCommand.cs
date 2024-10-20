using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwapCommand : AttackCommand
{
    int targetIndex = 0;
    public SwapCommand() : base()
    {
        skippable = false;
        canFocus = false;
    }


    public override void ExecuteCommand()
    {



        Debug.Log(Source.name + " : Was Position : " + BattleManager.Singleton.GetActorIndex(Source));
        Debug.Log(Source.name + " : Wanted Position is : " + targetIndex);

        BattleManager.Singleton.SetActorIndex(Source, targetIndex);
        foreach (BattleCharacter bc in BattleManager.Singleton.GetPartyOf(BattleManager.Singleton.GetActor()))
        {
            Source.StartCoroutine(GoToOriginalPosition(bc));
        }
        Source.StartCoroutine(Execute());
        Debug.Log(Source.name + " : Is Now Position : " + BattleManager.Singleton.GetActorIndex(Source));
    }

    public override IEnumerator Execute()
    {
        yield return new WaitForSeconds(2f);
        OnExecuted?.Invoke();

    }


    public void SetTargetIndex(int actorsIndex)
    {
        targetIndex = actorsIndex;
    }
}
