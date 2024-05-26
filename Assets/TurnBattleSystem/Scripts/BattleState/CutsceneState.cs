using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneState : BattleState
{

    public override void OnEnter(BattleManager _battleManager)
    {
        base.OnEnter(_battleManager);
        battleManager.SetCursor(null);
    }


    public override void Handle()
    {
        base.Handle();
    }


    public void PerformanceOver()
    {

    }


    public override void OnExit()
    {
        base.OnExit();
    }
}
