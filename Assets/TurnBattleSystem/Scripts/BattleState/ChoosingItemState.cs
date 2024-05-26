using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChoosingItemState : ChoosingSkillState
{

    

    public override void ShowControls()
    {
        battleManager.SetControlText("WASD to navigate | Left Click to Select Item | Right Click to return");
    }


    public override void InstantiateMenu(BattleCharacter character)
    {
        GameObject choiceMenuPrefab = Resources.Load<GameObject>("ItemMenu");
        if (choiceMenuPrefab != null)
        {
            choiceMenu = GameObject.Instantiate(choiceMenuPrefab,GameObject.Find("HUD Canvas").transform);
            
        }
        else
        {
            Debug.LogError("ChoiceMenu prefab not found in Resources.");
        }


        choiceMenu.GetComponent<ItemMenu>().AddButtons(battleManager.GetPlayerItems());
    }
}
