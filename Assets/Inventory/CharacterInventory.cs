using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Inventory", menuName = "Inventory/CharacterInventory")]
public class CharacterInventory : ScriptableObject
{
    public List<InventoryItem> myInventory = new List<InventoryItem>();
    public int pessos = 0;

    public void AddMoney(int amount)
    {
        pessos += amount;
    }

    public bool CheckInventoryFor(List<InventoryItem> requiredItems)
    {
        List<InventoryItem> tempInv = myInventory;
        foreach (InventoryItem item in requiredItems)
        {
            if (tempInv.Contains(item))
            {
                tempInv.Remove(item);
            }
            else
            {
                return false;
            }
        }
        return true;
    }

    public void AddToInventory(InventoryItem content, int amount = 1)
    {
            if (content.itemName == "pesso")
            {
                    AddMoney(amount);
            }
            else
            {
                for (int i = 0; i < amount; i++)
                {
                    myInventory.Add(content);
                }
            }
        
    }
    public List<InventoryItem> ReturnUniqueInventory()
    {
        List<InventoryItem> temp = new List<InventoryItem>();
        foreach(InventoryItem ii in myInventory)
        {
            if (!temp.Contains(ii))
            {
                temp.Add(ii);
            }
        }

        return temp;
    }

    public bool HasObject(InventoryItem content, int amount = 1)
    {
        List<InventoryItem> occurrences = myInventory.FindAll(x => x == content);
        if (occurrences.Count < amount)
        {
            return false;
        }
        else
        {
            return true;
        }
    }


    public int AmountObject(InventoryItem content)
    {
        List<InventoryItem> occurrences = myInventory.FindAll(x => x == content);
        return occurrences.Count;

    }
    public bool RemoveFromInventory(InventoryItem content, int amount = 1)
    {

        List<InventoryItem> occurrences = myInventory.FindAll(x => x == content);
        if (occurrences.Count < amount)
        {
            return false;
        }
        for (int i = 0; i < amount; i++)
        {
            myInventory.Remove(content);
        }
        return true; ;
    }
}
