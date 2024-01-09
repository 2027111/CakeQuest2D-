using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HurtState : MeleeBaseState
{
    public override void OnEnter(StateMachine _stateMachine)
    {
        base.OnEnter(_stateMachine);
        animator.SetTrigger("Hurt");
        duration = Random.Range(0.2f, 0.3f);
    }





    public override void OnUpdate()
    {

        if (fixedtime > duration)
        {
            stateMachine.SetNextStateToMain();

        }
    }
}
