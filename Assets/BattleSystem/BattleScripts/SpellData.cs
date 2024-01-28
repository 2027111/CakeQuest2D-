using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AttackProperties
{
    None,
    FriendlyTarget,
    EnemyTarget
}
public enum SpawnBehaviour
{
    SpawnOnCaster,
    SpawnOnTarget
}

[CreateAssetMenu(fileName = "New SpellData", menuName = "SpellData")]
[Serializable]



public class SpellData : MoveData
{



    public AttackProperties attackProperties = AttackProperties.EnemyTarget;
    public float castingTime = 5;
    public float spellDuration = 3;
    public float SpellIntermissionTime = .2f;
    public SpawnBehaviour spawnBehaviour = SpawnBehaviour.SpawnOnCaster;
    public List<GameObject> SpellPrefabs;
    public GameObject SpellChargePrefab;
    public AudioClip ChargeSFX;
    public AudioClip ChargeVoiceLine;
    public Vector2 XpositionOffsetRange = Vector2.zero;
    public Vector2 YpositionOffsetRange = Vector2.zero;
    public float rotationOffset = 0;



    public AudioClip GetChargeEffect()
    {
        if (ChargeSFX)
        {
            return ChargeSFX;
        }
        else
        {
            return null;
        }
    }
    public AudioClip GetChargeVoiceline()
    {
            return ChargeVoiceLine ? ChargeVoiceLine : null;
    }
}
