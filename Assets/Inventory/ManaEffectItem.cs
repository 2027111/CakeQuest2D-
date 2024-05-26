using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Mana Item", menuName = "Inventory/Mana Items")]
public class ManaEffectItem : BattleItem
{
    public int manaEffectAmount = 12;

    public override int GetAmount()
    {
        return manaEffectAmount;
    }
    public override void BattleUse(List<BattleCharacter> Target)
    {
        foreach (BattleCharacter target in Target)
        {
            target?.Entity.AddToMana(manaEffectAmount);
        }
    }
}
