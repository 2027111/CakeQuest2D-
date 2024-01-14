using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;




[CreateAssetMenu(fileName = "New SpellData", menuName = "SpellData")]
[Serializable]



public class SpellData : AttackData
{



    public AttackProperties attackProperties = AttackProperties.EnemyTarget;
    public float castingTime = 5;
    public float spellDuration = 5;

    
}
