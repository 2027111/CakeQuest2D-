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
    
}
