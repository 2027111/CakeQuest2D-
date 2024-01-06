using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum ItemType
{
    Consumable,
    Weapon,
    Armor,
    Key,
    Ingredient,

}
[CreateAssetMenu]

[System.Serializable]
public class Item : ScriptableObject
{
    public string itemName;
    public string itemDescription;
    public Sprite itemSprite;
    public ItemType itemType;
}
