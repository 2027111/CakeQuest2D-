using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerformActionState : BattleState
{
    GameObject choiceMenu;
    BattleCharacter performer;
    public override void OnEnter(BattleManager _battleManager)
    {
        base.OnEnter(_battleManager);
        battleManager.SetCursor(null);

        performer = battleManager.GetActor();
        performer.Animator.Thinking(false);
        performer.SetActing(true);
        if (!battleManager.NextActorIsSameTeam() || !battleManager.NextActorIsPlayer() || !performer.currentCommand.skippable)
        {

            performer.currentCommand.OnExecuted += PerformanceOver;
            battleManager.StartCoroutine(CheckFocus());

        }
        performer.currentCommand.OnExecuted += delegate { performer.SetActing(false); };
        performer.currentCommand.OnExecuted += performer.ResetAnimatorController;
        performer.currentCommand.ExecuteCommand();
        CamManager.ResetView();
    }


    public override void Handle()
    {

        if (battleManager.GetActor().currentCommand.skippable && battleManager.NextActorCanAct() && battleManager.NextActorIsPlayer() && battleManager.NextActorIsSameTeam())
        {
            performer.currentCommand.OnExecuted -= PerformanceOver;
            PerformanceOver();

        }




        base.Handle();
    }

    public override void ShowControls()
    {
        if (battleManager.GetActor())
        {
            string controls = LanguageData.GetDataById("ControlScheme").GetValueByKey(this.GetType().ToString());

            if (battleManager.GetActor().IsPlayerTeam())
            {
                 controls = LanguageData.GetDataById("ControlScheme").GetValueByKey("NothingState");

            }
            battleManager.SetControlText(controls);
        }
    }

    IEnumerator CheckFocus()
    {

        if (performer.currentCommand.CanFocus())
        {

        List<BattleCharacter> takeOvers = new List<BattleCharacter>();
        if (!battleManager.IsForcedTurn())
        {

        yield return new WaitForSeconds(Random.Range(.3f, .9f));





        if(performer.GetTeam() == TeamIndex.Player)
        {
            foreach(BattleCharacter bc in battleManager.HeroPartyActors)
            {
                if(bc.CanAct() && bc.Entity.Focus >= 10 && bc != battleManager.GetActor())
                {
                    takeOvers.Add(bc);
                }
            }
        }


        if (takeOvers.Count > 0)
        {
            InstantiateMenu(takeOvers);
            yield return new WaitForSeconds(Random.Range(.6f, 1.9f));
            if (choiceMenu)
            {
                choiceMenu.GetComponent<TakeOverMenu>().DestroyMenu();
            }
        }



        }
        yield return null;
        }
    }

    public override void OnSelect()
    {
        if (choiceMenu)
        {
            choiceMenu.GetComponent<ChoiceMenu>().TriggerSelected();
        }
        base.OnSelect();
    }
    public override void OnNavigate(Vector2 direction)
    {
        if (choiceMenu)
        {
            if (direction.x > 0)
            {
                choiceMenu.GetComponent<ChoiceMenu>().NextButton();
            }
            else if (direction.x < 0)
            {

                choiceMenu.GetComponent<ChoiceMenu>().PreviousButton();
            }
        }
        base.OnNavigate(direction);
    }

    public override void OnBack()
    {
        if (battleManager.IsEnemyTurn())
        {
            foreach(BattleCharacter bc in battleManager.HeroPartyActors)
            {
                if (!bc.isActing && bc.IsTargetted())
                {
                    bc.Block();
                }
            }
        }
        base.OnBack();
    }

    public override void OnBackReleased()
    {
        if (battleManager.IsEnemyTurn())
        {
            foreach (BattleCharacter bc in battleManager.HeroPartyActors)
            {

                    bc.StopBlock();
                
            }
        }
        base.OnBackReleased();
    }
    public void InstantiateMenu(List<BattleCharacter> battleCharacters)
    {
        GameObject choiceMenuPrefab = Resources.Load<GameObject>("TakeOverMenu");
        if (choiceMenuPrefab != null)
        {
            choiceMenu = GameObject.Instantiate(choiceMenuPrefab, GameObject.Find("HUD Canvas").transform);

        }
        else
        {
            Debug.LogError("TakeOverMenu prefab not found in Resources.");
        }


        choiceMenu.GetComponent<TakeOverMenu>().GiveTakeOvers(battleCharacters, this);
    }



    public void ForceTurn(BattleCharacter battleCharacter)
    {
        if (battleManager.GetActor().currentCommand != null)
        {
            battleManager.GetActor().currentCommand.OnExecuted -= PerformanceOver;
        }
        battleManager.SetActor(battleCharacter);
        battleCharacter.Entity.AddFocus(-100);
        battleManager.StartNewTurn();
    }
    public void PerformanceOver()
    {
        battleManager.StopCoroutine(CheckFocus());
        battleManager.NextTurn();
        battleManager.StartNewTurn();
    }


    public override void OnExit()
    {
        base.OnExit();
        battleManager.StopCoroutine(CheckFocus());
        foreach (BattleCharacter bc in battleManager.HeroPartyActors)
        {

            bc.StopBlock();
        }
        if (battleManager.GetActor().currentCommand != null)
        {
            battleManager.GetActor().currentCommand.OnExecuted -= PerformanceOver;
        }
        battleManager.GetActor().Animator.Thinking(false);
    }
}