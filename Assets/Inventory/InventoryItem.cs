using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
[System.Serializable]
[CreateAssetMenu(fileName = "New Item", menuName ="Inventory/Items")]
public class InventoryItem : IActionData
{

    public string itemName;
    public string itemId;
    public string itemDescription;
    public Sprite itemSprite;
    public bool usable;
    public bool unique;

    public string GetName()
    {
        string name = itemId;
        string newName = LanguageData.GetDataById("item_" + itemId).GetValueByKey("skillName");
        if (newName != "E404")
        {
            return newName;
        }
        return name;
    }

    public string GetDescription()
    {
        string desc = itemDescription;
        string newDesc = LanguageData.GetDataById("item_" + itemId).GetValueByKey("skillDescription");
        if (newDesc != "E404")
        {
            return newDesc;
        }
        return desc;
    }
    public virtual void BattleUse(List<BattleCharacter> Target, CharacterInventory inventory = null)
    {
        if (inventory)
        {
            inventory.RemoveFromInventory(this);
        }
    }


    public virtual void OverWorldUse(List<CharacterObject> Target, CharacterInventory inventory = null)
    {
        if (inventory)
        {
            inventory.RemoveFromInventory(this);
        }
    }

}
