using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Support Skill", menuName = "Skills/SupportSkill")]
public class SupportSkill : Skill
{
    public override void UseSkill(BattleCharacter Source, List<BattleCharacter> Target)
    {
        Debug.Log("Lol");
    }
}
