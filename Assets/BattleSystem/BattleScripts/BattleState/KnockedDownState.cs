using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnockedDownState : MeleeBaseState
{
    public override void OnEnter(StateMachine _stateMachine)
    {
        base.OnEnter(_stateMachine);
        animator.SetTrigger("Dead");
        duration = Random.Range(2f, 3f);
        stateMachine.SetLayerRecursively(8, stateMachine.gameObject);
        cc.entity.characterObject.Armor = cc.entity.characterObject.BaseArmor;
        cc.entity.damageChain = 0;
    }





    public override void OnUpdate()
    {

        if (fixedtime > duration)
        {
            stateMachine.SetNextState(new GetUpState());

        }
    }

    public override void OnExit()
    {
        base.OnExit();





    }
}
