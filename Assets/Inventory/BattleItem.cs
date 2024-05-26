using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
[CreateAssetMenu(fileName = "New Battle Item", menuName ="Inventory/Battle Items")]
public class BattleItem : InventoryItem
{

    public override void BattleUse(List<BattleCharacter> target)
    {
        Debug.Log("Yus");
    }


}
