using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Healing Item", menuName = "Inventory/Healing Items")]
public class HealthEffectItem : BattleItem
{
    public int healthEffectAmount = 12;
    public override int GetAmount()
    {
        return healthEffectAmount;
    }
    public override void BattleUse(List<BattleCharacter> Target, CharacterInventory inventory = null)
    {
        foreach (BattleCharacter target in Target)
        {
            target?.Entity.AddToHealth(this, ElementEffect.Neutral, BattleManager.Singleton.GetActor());
        }
        base.BattleUse(Target, inventory);
    }
}
