using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetUpState : MeleeBaseState
{
    public override void OnEnter(StateMachine _stateMachine)
    {



        
        base.OnEnter(_stateMachine);
        
        float x = cc.direction.x;
        if (x > 0)
        {
            cc.side = 1;
            cc.Flip(cc.side);
        }
        if (x < 0)
        {
            cc.side = -1;
            cc.Flip(cc.side);
        }

        cc.rb.AddForce(Vector2.right * 400 * cc.side);
        animator.SetTrigger("Roll");
        duration = animator.GetCurrentAnimatorClipInfo(0).Length * .85f;
        cc.entity.characterObject.Armor = cc.entity.characterObject.BaseArmor;
        cc.entity.damageChain = 0;
        cc.gameObject.layer = 9;











    }





    public override void OnUpdate()
    {

        if (fixedtime > duration)
        {
            stateMachine.SetNextStateToMain();

        }
    }

    public override void OnExit()
    {
        base.OnExit();




    }
}
