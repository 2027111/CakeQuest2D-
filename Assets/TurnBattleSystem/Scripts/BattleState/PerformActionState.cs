using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerformActionState : BattleState
{
    GameObject choiceMenu;

    public override void OnEnter(BattleManager _battleManager)
    {
        base.OnEnter(_battleManager);
        battleManager.SetCursor(null);

       
        if (!battleManager.NextActorIsSameTeam() || !battleManager.NextActorIsPlayer())
        {

            battleManager.GetActor().currentCommand.OnExecuted += PerformanceOver;
            battleManager.StartCoroutine(CheckFocus());

        }
        battleManager.GetActor().currentCommand.OnExecuted += delegate { battleManager.GetActor().SetActing(false); };
        battleManager.GetActor().currentCommand.OnExecuted += battleManager.GetActor().ResetAnimatorController;
        battleManager.GetActor().currentCommand.ExecuteCommand();
        CamManager.ResetView();
    }


    public override void Handle()
    {

        if (battleManager.NextActorCanAct() && battleManager.NextActorIsPlayer() && battleManager.NextActorIsSameTeam())
        {

            PerformanceOver();

        }




        base.Handle();
    }

  

    IEnumerator CheckFocus()
    {
        List<BattleCharacter> takeOvers = new List<BattleCharacter>();
        if (!battleManager.IsForcedTurn())
        {

        yield return new WaitForSeconds(Random.Range(.3f, .9f));





        if(battleManager.GetActor().GetTeam() == TeamIndex.Player)
        {
            foreach(BattleCharacter bc in battleManager.HeroPartyActors)
            {
                if(!bc.isActing && bc.Entity.Focus >= 10 && bc != battleManager.GetActor())
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
            Debug.Log("Block");
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
            Debug.Log("Stopped block");
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
    }
}
