using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Fetch Quest", menuName = "Quests/FetchQuests")]
public class FetchQuest : QuestObject
{



    public List<InventoryItem> requiredItems = new List<InventoryItem>();
    public CharacterInventory characterInventory;
    
    public override void CheckConditions()
    {
        if (characterInventory.CheckInventoryFor(requiredItems))
        {
            Debug.Log("Quest was a success!");
            CompleteQuest();
        }
        base.CheckConditions();
    }
}