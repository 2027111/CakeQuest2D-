using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellCastingState : AttackState
{
    bool casting;
    GameObject SpellCastingObject;
    List<GameObject> SpellObjects = new List<GameObject>();
    GameObject Target;
    public SpellCastingState() : base()
    {
    }

    public void OnEnter(StateMachine _stateMachine, SpellData spellData)
    {
        base.OnEnter(_stateMachine, spellData);


        currentData = spellData;
        animator.SetTrigger("Casting");
        casting = true;
        SpawnParticleCharge((SpellData)currentData);
        DebugGetTarget();
        cc.entity.AddToMana(-currentData.manaCost);
        Debug.Log(Target.name);

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
    IEnumerator SpawnSpell(SpellData data)
    {
        if (data)
        {
            if (data.SpellPrefabs.Count > 0)
            {
                foreach (GameObject spellPrefab in data.SpellPrefabs)
                {
                    Vector3 spawnPosition = GetTargetSpawn(data);
                    GameObject SpellObject = GameObject.Instantiate(spellPrefab, spawnPosition, Quaternion.identity);
                    SpellObject.GetComponent<Projectile>().SetDirection(cc.GetFacing());
                    SpellObject.GetComponent<Projectile>().SetDuration(data.spellDuration);
                    SpellObject.GetComponent<Projectile>().SetOwner(cc);
                    SpellObjects.Add(SpellObject);
                    yield return new WaitForSecondsRealtime(data.SpellIntermissionTime);
                }
            }


        }
    }

    private Vector3 GetCharacterFlipSide()
    {
        Vector3 vector = Vector3.one;
        vector.x = cc.GetFacing();
        return vector;
    }

    private Vector3 GetTargetSpawn(SpellData data)
    {
        Vector3 randomize = data.SpellPrefabs.Count > 0?new Vector3(Random.Range(-1.1f, 1.1f), Random.Range(0f, 1.1f), 0):Vector3.zero;
        Vector3 position = Vector3.zero;
        switch (data.spawnBehaviour)
        {
            case SpawnBehaviour.SpawnOnTarget:
                position =  Target.transform.position ;
                break;
            case SpawnBehaviour.SpawnOnCaster:
                position =  stateMachine.transform.position;
                break;
            default:
                position =  stateMachine.transform.position;
                break;
        }

        return position + randomize;
    }

    private void UnspawnSpell()
    {
        if (SpellObjects.Count > 0)
        {

            foreach (GameObject SpellObject in SpellObjects)
            {
                if (SpellObject != null)
                {
                    GameObject.Destroy(SpellObject);
                }
            }
        }

    }

    public void DebugGetTarget()
    {
        BattleCharacter[] bcs = GameObject.FindObjectsOfType<BattleCharacter>();
        foreach(BattleCharacter bc in bcs)
        {
            if (bc != cc && (bc.GetComponent<TeamComponent>().teamIndex != cc.GetComponent<TeamComponent>().teamIndex))
            {
                Target = bc.gameObject;
                break;
            }
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

        if (fixedtime > ((SpellData)currentData).castingTime)
        {
            if (casting)
            {

                animator.SetTrigger("DoneCasting");
                casting = false;
                stateMachine.StartCoroutine(SpawnSpell(((SpellData)currentData)));
                UnspawnParticleCharge();
            }

            

        }

        if (fixedtime > ((SpellData)currentData).castingTime + ((SpellData)currentData).spellDuration + (((SpellData)currentData).SpellIntermissionTime * ((SpellData)currentData).SpellPrefabs.Count))
        {
            stateMachine.SetNextStateToMain();
        }
    }

    public override void OnExit()
    {
        base.OnExit();
        animator.SetTrigger("SpellOver");
        UnspawnSpell();
        UnspawnParticleCharge();

    }

    
}
