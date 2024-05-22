using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefensiveSkillCommand : SkillCommand
{
    public DefensiveSkillCommand(Attack _attack) : base(_attack)
    {
    }





    public override void ActivateCommand()
    {
        foreach (BattleCharacter target in Target)
        {
            target?.Entity.TakeDamage(attack.baseDamage, ElementEffect.Neutral);
        }
    }

}
