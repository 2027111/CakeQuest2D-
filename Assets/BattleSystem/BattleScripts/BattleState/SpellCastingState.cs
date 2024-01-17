using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellCastingState : MeleeBaseState
{
    new SpellData currentSData;
    bool casting;
    GameObject SpellCastingObject;
    public SpellCastingState() : base()
    {
    }

    public void OnEnter(StateMachine _stateMachine, SpellData spellData)
    {
        base.OnEnter(_stateMachine);
        currentSData = spellData;
        animator.SetTrigger("Casting");
        casting = true;
        SpawnParticleCharge(currentSData);
        Debug.Log(currentSData.spellDuration);

    }




    public void SpawnParticleCharge(SpellData data)
    {
        if(SpellCastingObject == null)
        {
            if (data)
            {
                if (data.SpellChargePrefab)
                {
                    SpellCastingObject = Object.Instantiate(data.SpellChargePrefab, stateMachine.transform);
                }
            }

        }

    }


    public void UnspawnParticleCharge()
    {

        if (SpellCastingObject != null)
        {
            GameObject.Destroy(SpellCastingObject);
        }
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
                UnspawnParticleCharge();
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
