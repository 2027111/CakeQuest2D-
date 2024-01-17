using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellCastingState : MeleeBaseState
{
    SpellData currentSData;
    bool casting;
    GameObject SpellCastingObject;
    GameObject SpellObject;
    GameObject Target;
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




    private void SpawnParticleCharge(SpellData data)
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
    private void SpawnSpell(SpellData data)
    {
        if (SpellObject == null)
        {
            if (data)
            {
                if (data.SpellPrefab)
                {
                    Transform spawnTarget = GetTargetSpawn(data);

                    SpellObject = Object.Instantiate(data.SpellPrefab, spawnTarget.position, Quaternion.identity);
                }
            }

        }

    }

    private Transform GetTargetSpawn(SpellData data)
    {
        switch (data.spawnBehaviour)
        {
            case SpawnBehaviour.SpawnOnTarget:
                return Target.transform;
            case SpawnBehaviour.SpawnOnCaster:
                return stateMachine.transform;
            default:
                return stateMachine.transform;
        }
    }

    private void UnspawnSpell()
    {
        if (SpellObject != null)
        {
            GameObject.Destroy(SpellObject);
        }

    }


    private void UnspawnParticleCharge()
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
                SpawnSpell(currentSData);
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
        UnspawnSpell();

    }

    
}
