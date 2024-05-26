using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwapingState : BattleState
{

    GameObject choiceMenu;
    public override void OnEnter(BattleManager _battleManager)
    {
        base.OnEnter(_battleManager);
        battleManager.GetActor().currentCommand = null;
        CamManager.PanToCharacter(battleManager.GetActor());
        InstantiateMenu();
    }


    public override void Handle()
    {
        base.Handle();
    }
    public override void ShowControls()
    {
            battleManager.SetControlText("");
    }
    public override void OnSelect()
    {
        choiceMenu.GetComponent<ChoiceMenu>().TriggerSelected();
        base.OnSelect();
    }

    public override void OnBack()
    {

        battleManager.ChangeState(new ChoosingActionState());
        base.OnBack();
    }
    public override void OnNavigate(Vector2 direction)
    {
        if (direction.x < 0)
        {
            choiceMenu.GetComponent<ChoiceMenu>().PreviousButton();
        }
        else if (direction.x > 0)
        {

            choiceMenu.GetComponent<ChoiceMenu>().NextButton();
        }

        base.OnNavigate(direction);

    }
    public void PerformanceOver()
    {

    }

    public void InstantiateMenu()
    {
        GameObject choiceMenuPrefab = Resources.Load<GameObject>("SwapMenu");
        if (choiceMenuPrefab != null)
        {
            choiceMenu = GameObject.Instantiate(choiceMenuPrefab, GameObject.Find("HUD Canvas").transform);

        }
        else
        {
            Debug.LogError("SwapMenu prefab not found in Resources.");
        }


        choiceMenu.GetComponent<SwappingMenu>().AddButtons(battleManager.GetPartyOf(battleManager.GetActor()));
    }
    public override void OnExit()
    {
        GameObject.Destroy(choiceMenu);
        base.OnExit();
    }
}
