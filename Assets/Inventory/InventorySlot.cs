using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class InventorySlot : MonoBehaviour
{
    [Header("UI Stuff to change")]
    [SerializeField] TMP_Text itemNameText;
    [SerializeField] TMP_Text amountText;
    [SerializeField] Image itemLogo;



    [Header("Variables from the item")]
    public Sprite itemSprite;
    public string itemName;
    public int itemAmount;
    public string itemDescription;
    public InventoryItem thisItem;
    public InventoryManager thisManager;

    public UnityEvent OnSelect;

    public void Setup(InventoryItem newItem, InventoryManager newManager, int amount = 1)
    {
        thisItem = newItem;
        thisManager = newManager;


        if (thisItem)
        {
            itemSprite = thisItem.itemSprite;
            itemName = thisItem.GetName();
            itemAmount = amount;
            amountText.text = $"x{itemAmount}";
            itemNameText.text = itemName;
            itemLogo.sprite = itemSprite;
            itemDescription = thisItem.GetDescription();

        }

    }

    public void ClickedOn()
    {
        if (thisItem)
        {
            thisManager.SetupDescriptionAndButton(thisItem);
        }
    }


    public void Selected()
    {
        OnSelect?.Invoke();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
