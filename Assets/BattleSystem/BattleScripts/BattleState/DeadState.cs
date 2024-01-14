using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DeadState : State
{
    public static float DeadTime = 3;
    public override void OnEnter(StateMachine _stateMachine)
    {
        base.OnEnter(_stateMachine);
        cc.canMove = false;
        cc.rb.velocity = Vector3.zero;
        cc.GetComponent<Entity>().characterObject.isDead = true;
        cc.gameObject.layer = 13;
        animator.SetTrigger("Dead");
    }




    public override void OnExit()
    {
        base.OnExit();
        cc.canMove = true;
        cc.gameObject.layer = 9;
        animator.Rebind();
        animator.Update(0);
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
    }
}