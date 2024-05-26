using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
[CreateAssetMenu(fileName = "New Battle Item", menuName ="Inventory/Battle Items")]
public class BattleItem : InventoryItem
{

    public override void BattleUse(List<BattleCharacter> target, CharacterInventory inventory = null)
    {
        Debug.Log("Yus");
    }


}
