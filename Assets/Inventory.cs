using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Inventory : ScriptableObject
{

    public List<Item> items = new List<Item>();
    public int pessos = 0;

    public void AddMoney(int amount)
    {
        pessos += amount;
    }
}
