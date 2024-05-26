using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChoosingActionState : BattleState
{

    GameObject choiceMenu;

    public override void OnEnter(BattleManager _battleManager)
    {

        base.OnEnter(_battleManager);
        battleManager.GetActor().currentCommand = null;
        battleManager.GetActor().Animator.Thinking(true);
        battleManager.SetCursor(battleManager.GetActor());
        CamManager.PanToCharacter(battleManager.GetActor());
        InstantiateMenu(battleManager.GetActor());
    }

    public override void ShowControls()
    {
        battleManager.SetControlText("WASD to navigate | Left Click to Select Action");
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
        if (direction.y < 0)
        {
            choiceMenu.GetComponent<ChoiceMenu>().NextButton();
        }
        else if (direction.y > 0)
        {

            choiceMenu.GetComponent<ChoiceMenu>().PreviousButton();
        }

        base.OnNavigate(direction);

    }
    public override void OnExit()
    {
        base.OnExit();
        GameObject.Destroy(choiceMenu);
    }



    public void InstantiateMenu(BattleCharacter character)
    {
        GameObject choiceMenuPrefab = Resources.Load<GameObject>("BattleMenu");
        if (choiceMenuPrefab != null)
        {
            choiceMenu = GameObject.Instantiate(choiceMenuPrefab, character.transform.position + Vector3.up, Quaternion.identity);
        }
        else
        {
            Debug.LogError("ChoiceMenu prefab not found in Resources.");
        }
    }
}
