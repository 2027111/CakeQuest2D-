using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
[CreateAssetMenu(fileName = "New Battle Item", menuName ="Inventory/Battle Items")]
public class BattleItem : InventoryItem
{
    public TargetType targetType = TargetType.Single;
    public Friendliness friendliness = Friendliness.Non_Friendly;
    public TargetStateType targetStateType = TargetStateType.Alive;
    public Element element = Element.Slash;

    public override void BattleUse(List<BattleCharacter> target)
    {

    }

}
