﻿using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemButton : ChoiceMenuButton
{

    public BattleItem storedItem;
    int itemAmount = 1;

    [SerializeField] TMP_Text itemNameText;
    [SerializeField] TMP_Text itemDescText;



    [SerializeField] TMP_Text itemAmountText;
    [SerializeField] Image attackTypeLogo;
    Vector2 baseSize = new Vector2(160, 40);


    public bool IsItem(InventoryItem obj)
    {
        return storedItem == obj;
    }
    public void Add()
    {
        itemAmount++;
        itemAmountText.text ="x"+itemAmount.ToString();
    }
    public void SetItem(BattleItem item)
    {
        storedItem = item;


        itemNameText.text = storedItem.itemName;
        itemDescText.text = storedItem.itemDescription;
        itemAmountText.text = "x" + itemAmount.ToString();
        Sprite itemSprite = storedItem.itemSprite;
        if (itemSprite)
        {
            attackTypeLogo.sprite = itemSprite;
        }
    }

    public override void OnOver(bool isOverHanded)
    {
        RectTransform trans = transform.GetComponent<RectTransform>();
        Vector2 vec = baseSize;
        if (isOverHanded)
        {
            vec = new Vector2(baseSize.x, baseSize.y * 2f);
        }
        trans.sizeDelta = vec;

    }


}
