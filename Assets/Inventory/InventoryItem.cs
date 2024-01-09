using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public enum ItemType
{
    Consumable,
    Weapon,
    Armor,
    Key,
    Ingredient,

}
[System.Serializable]
[CreateAssetMenu(fileName = "New Item", menuName ="Inventory/Items")]
public class InventoryItem : ScriptableObject
{

    public string itemName;
    public string itemDescription;
    public ItemType itemType;
    public Sprite itemSprite;
    public bool usable;
    public bool unique;
    public UnityEvent OnUseEvent;


    public void Use()
    {
        Debug.Log("Using Item");
        OnUseEvent?.Invoke();
    }

}
