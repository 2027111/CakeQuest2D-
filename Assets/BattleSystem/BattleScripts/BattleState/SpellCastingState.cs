using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellCastingState : MeleeBaseState
{
    new SpellData currentSData;
    bool casting;
    public SpellCastingState() : base()
    {
    }

    public void OnEnter(StateMachine _stateMachine, SpellData spellData)
    {
        base.OnEnter(_stateMachine);
        currentSData = spellData;
        animator.SetTrigger("Casting");
        casting = true;

        Debug.Log(currentSData.spellDuration);

    }





    public override void OnUpdate()
    {

        if (fixedtime > currentSData.castingTime)
        {
            Debug.Log("Spell Over");
            if (casting)
            {

                animator.SetTrigger("DoneCasting");
                casting = false;
            }

            

        }

        if (fixedtime > currentSData.castingTime + currentSData.spellDuration)
        {
            stateMachine.SetNextStateToMain();
        }
    }

    public override void OnExit()
    {
        base.OnExit();
        animator.SetTrigger("SpellOver");

    }

    
}
