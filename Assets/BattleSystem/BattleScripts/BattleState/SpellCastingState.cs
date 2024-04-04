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
        PlayChargeSFXs();
        PlayChargeVoiceLines();

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
                    SpellObject.GetComponent<Projectile>().SetDuration(data.spellDuration * 12);
                    SpellObject.GetComponent<Projectile>().SetOwner(cc);
                    if(data.spawnBehaviour == SpawnBehaviour.SpawnOnTarget)
                    {
                        SpellObject.GetComponent<Projectile>().SetTarget(Target.transform);
                    }
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
        Vector3 randomize = new Vector3(Random.Range(data.XpositionOffsetRange.x, data.XpositionOffsetRange.y), Random.Range(data.YpositionOffsetRange.x, data.YpositionOffsetRange.y), 0);
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
                SpawnAttackName();
                PlaySFXs();
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
        if (((SpellData)currentData).killWithSpellEnd)
        {
            UnspawnSpell();
        }
        UnspawnParticleCharge();

    }

    private void PlayChargeSFXs()
    {

        if (((SpellData)currentData).GetChargeEffect())
        {
            cc.PlaySFX(((SpellData)currentData).GetChargeEffect());
        }
    }
    private void PlayChargeVoiceLines()
    {

        if (((SpellData)currentData).GetChargeVoiceline())
        {
            cc.PlayVoiceclip(((SpellData)currentData).GetChargeVoiceline());
        }
    }
}
