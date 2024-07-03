using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Healing Item", menuName = "Inventory/Items/Healing Items")]
public class HealthEffectItem : BattleItem
{
    [JsonIgnore] public int healthEffectAmount = 12;
    public override int GetAmount()
    {
        return healthEffectAmount;
    }
    public override void BattleUse(List<BattleCharacter> Target, CharacterInventory inventory = null)
    {
        foreach (BattleCharacter target in Target)
        {
            AttackInformation attackInfo = new AttackInformation(this, ElementEffect.Neutral, BattleManager.Singleton.GetActor(), BattleManager.Singleton.GetActor().currentCommand);
            target?.Entity.AddToHealth(attackInfo);
        }
        base.BattleUse(Target, inventory);
    }
}
