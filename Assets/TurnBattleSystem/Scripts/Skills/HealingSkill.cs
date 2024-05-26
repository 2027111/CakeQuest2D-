
using System.Collections;
using System.Collections.Generic;
using UnityEngine;




[CreateAssetMenu(fileName = "Healing Skill", menuName = "Skills/Healing Skill")]
public class HealingSkill : Skill
{
   



    public override void UseSkill(BattleCharacter Source, List<BattleCharacter> Target)
    {
        foreach (BattleCharacter target in Target)
        {
            target?.Entity.AddToHealth(this, ElementEffect.Neutral, Source);
        }
    }
}
