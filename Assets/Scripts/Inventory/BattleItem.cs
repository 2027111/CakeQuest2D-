using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
[CreateAssetMenu(fileName = "New Battle Item", menuName ="Inventory/Items/Battle Items")]
public class BattleItem : InventoryItem
{

    public override void BattleUse(List<BattleCharacter> Target, CharacterInventory inventory = null)
    {
        base.BattleUse(Target, inventory);
    }


}
