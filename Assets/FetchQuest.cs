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

    public override string GetObjectiveProgress()
    {
        string returnValue = "";
        if (RuntimeValue)
        {
            returnValue = "done"; 
            string newDesc = LanguageData.GetDataById("Indications").GetValueByKey("done");
            if (newDesc != "E404")
            {
                returnValue = newDesc;
            }
        }
        else
        {
            Dictionary<InventoryItem, int> text = new Dictionary<InventoryItem, int>();
            foreach (InventoryItem ii in requiredItems)
            {
                if (text.TryGetValue(ii, out int value))
                {
                    text[ii] = value + 1;
                }
                else
                {
                    text.Add(ii, 1);
                }
            }
            foreach (KeyValuePair<InventoryItem, int> pait in text)
            {
                int amountinInv = characterInventory.AmountObject(pait.Key);
                returnValue += $"{pait.Key.GetName()} {amountinInv}/{pait.Value}";
                returnValue += "\n";
            }

        }
        return returnValue;

    }

}
