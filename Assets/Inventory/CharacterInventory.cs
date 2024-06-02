using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Inventory", menuName = "Inventory/CharacterInventory")]
public class CharacterInventory : SavableObject
{
    public List<InventoryItem> myInventory = new List<InventoryItem>();
    public int pessos = 0;



    public override void ApplyData(SavableObject tempCopy)
    {
        GameSaveManager.Singleton.StartCoroutine(AddLoadedItemToInventory((tempCopy as CharacterInventory).myInventory));
        pessos = (tempCopy as CharacterInventory).pessos;
        base.ApplyData(tempCopy);
    }

   
    public IEnumerator AddLoadedItemToInventory(List<InventoryItem> loadedInventory)
    {
        myInventory.Clear();


        foreach (InventoryItem item in loadedInventory)
        {
            ResourceRequest request = Resources.LoadAsync<InventoryItem>($"ItemFolder/{item.name}");
            while (!request.isDone)
            {
                yield return null;
            }
            InventoryItem loadedItem = request.asset as InventoryItem;
            AddToInventory(loadedItem);
            yield return null;
        }
        yield return null;
    }

    public void AddMoney(int amount)
    {
        pessos += amount;
    }

    public bool CheckInventoryFor(List<InventoryItem> requiredItems)
    {
        List<InventoryItem> tempInv = new List<InventoryItem>(myInventory);
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
