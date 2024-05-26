using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChoosingSkillState : BattleState
{

    public GameObject choiceMenu;

    public override void OnEnter(BattleManager _battleManager)
    {

        base.OnEnter(_battleManager);
        battleManager.GetActor().currentCommand = null;
        CamManager.PanToCharacter(battleManager.GetActor());
        InstantiateMenu(battleManager.GetActor());
    }
    public override void ShowControls()
    {
        battleManager.SetControlText("WASD to navigate | Left Click to Select Skill | Right Click to return");
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



    public virtual void InstantiateMenu(BattleCharacter character)
    {
        GameObject choiceMenuPrefab = Resources.Load<GameObject>("Skillmenu");
        if (choiceMenuPrefab != null)
        {
            choiceMenu = GameObject.Instantiate(choiceMenuPrefab,GameObject.Find("HUD Canvas").transform);
            
        }
        else
        {
            Debug.LogError("ChoiceMenu prefab not found in Resources.");
        }


        choiceMenu.GetComponent<SkillMenu>().AddButtons(battleManager.GetActor().GetAttacks());
    }
}
