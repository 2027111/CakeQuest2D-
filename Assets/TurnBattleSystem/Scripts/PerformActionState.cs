using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerformActionState : BattleState
{

    public override void OnEnter(BattleManager _battleManager)
    {
        base.OnEnter(_battleManager);
        battleManager.SetCursor(null);
        //battleManager.GetActor().currentCommand.OnExecuted += PerformanceOver;
        battleManager.GetActor().currentCommand.OnExecuted += delegate { battleManager.GetActor().SetActing(false); };
        battleManager.GetActor().currentCommand.OnExecuted += battleManager.GetActor().ResetAnimatorController;
        battleManager.GetActor().currentCommand.ExecuteCommand();

        CamManager.ResetView();
    }


    public override void Handle()
    {

        if (battleManager.NextPlayerCanMove())
        {

            battleManager.NextTurn();

        }

        base.Handle();
    }


    public void PerformanceOver()
    {


    }


    public override void OnExit()
    {
        base.OnExit();
        if (battleManager.GetActor().currentCommand != null)
        {
            battleManager.GetActor().currentCommand.OnExecuted -= PerformanceOver;
        }
    }
}
