using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwapingState : BattleState
{

    public SwapingState()
    {

        MenuName = "SwapMenu";
    }

    public override void OnEnter(BattleManager _battleManager)
    {
        base.OnEnter(_battleManager);
        battleManager.GetActor().currentCommand = null;
        InstantiateMenu(battleManager.GetActor());
    }


    public override void Handle()
    {
        base.Handle();
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

    public override void InstantiateMenu(BattleCharacter character)
    {
        base.InstantiateMenu(character);
        choiceMenu.GetComponent<SwappingMenu>().AddButtons(battleManager.GetPartyOf(character));
    }


    public override void OnMenuInstantiated()
    {
    }
    public override void OnExit()
    {
        GameObject.Destroy(choiceMenu);
        base.OnExit();
    }
}
