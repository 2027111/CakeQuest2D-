using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : State
{
    protected bool conserveVelocity = false;
    protected MoveData currentData;

    public override void OnEnter(StateMachine _stateMachine)
    {
        base.OnEnter(_stateMachine);
        cc.gameObject.layer = 9;
    }

    public void OnEnter(StateMachine _stateMachine, MoveData data)
    {
        base.OnEnter(_stateMachine);

        if (!data)
        {
            stateMachine.SetNextStateToMain();

        }
        else
        {

            currentData = data;
            cc.attackPlacement = currentData.attackPlacement;
            PlaySFXs();
            if (!currentData.conserveVelocity)
            {
                cc.rb.velocity = Vector3.zero;
            }
            cc.entity.AddToMana(-currentData.manaCost);
        }

    }

    private void PlaySFXs()
    {

        if (currentData.GetSoundEffect())
        {
            cc.PlaySFX(currentData.GetSoundEffect());
        }
        if (currentData.GetVoiceLine())
        {
            cc.PlayVoiceclip(currentData.GetVoiceLine());
        }
    }


   public override void OnExit()
    {
        base.OnExit();
        cc.attackPlacement = AttackPlacement.NONE;
    }




    public override void OnFixedUpdate()
    {
        base.OnFixedUpdate();
    }



    public override void OnLateUpdate()
    {
        base.OnLateUpdate();
    }





    
}
