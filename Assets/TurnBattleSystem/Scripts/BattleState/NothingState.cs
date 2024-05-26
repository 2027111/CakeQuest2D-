using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NothingState : BattleState
{

    public override void OnEnter(BattleManager _battleManager)
    {
        base.OnEnter(_battleManager);
        battleManager.SetCursor(null);
    }

    public override void ShowControls()
    {
        battleManager.SetControlText("Left Click -> Next");
    }
    public override void Handle()
    {
        base.Handle();
    }



    public override void OnExit()
    {
        base.OnExit();
    }
}
