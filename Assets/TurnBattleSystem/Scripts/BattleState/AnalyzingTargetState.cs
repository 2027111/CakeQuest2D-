using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnalyzingTargetState : BattleState
{
    public List<BattleCharacter> target = new List<BattleCharacter>();
    public List<BattleCharacter> possibleTarget;
    int targetIndex = 0;
    public override void OnEnter(BattleManager _battleManager)
    {
        base.OnEnter(_battleManager);
        Time.timeScale = 0f;
        battleManager.SetCursor(null);
        battleManager.actorInfoPanel.Appear(true);
            possibleTarget = battleManager.Actors;
            if (possibleTarget.Count == 0)
            {
                battleManager.ChangeState(new ChoosingActionState());
            }
            else
            {
                    Select(BattleManager.Singleton?.GetActor());
            }

    }

    public void Select(BattleCharacter character)
    {
                target.Clear();
                target.Add(character);
                battleManager.SetCursor(character);
                CamManager.PanToCharacter(character);
                battleManager.actorInfoPanel.SetActor(character);
                string t = LanguageData.GetDataById("Indications").GetValueByKey("targetOne");
                BattleManager.Singleton.SetIndicationText(t +" "+ character.name);
          
    }


    public override void Handle()
    {
        base.Handle();
    }
    public override void OnSelect()
    {

        if(battleManager.GetActor().Entity.HasMaxFocus() && target[0].GetTeam() == TeamIndex.Enemy)
        {
            target[0].RevealRecipe();
            battleManager.actorInfoPanel.SetActor(target[0]);
            battleManager.GetActor().Entity.AddFocus(-09999);
        }
        base.OnSelect();
    }

    public override void OnBack()
    {
        CamManager.ResetView();
        battleManager.ChangeState(new ChoosingActionState());
        base.OnBack();
    }

    public override void OnNavigate(Vector2 direction)
    {
            if (direction.x > 0)
            {
                NextTarget();
            }
            else if (direction.x < 0)
            {

                PreviousTarget(); 
            }

        base.OnNavigate(direction);
    }
    private void NextTarget()
    {
        
        targetIndex++;
        if(targetIndex >= possibleTarget.Count)
        {
            targetIndex = 0;
        }
        Select(possibleTarget[targetIndex]);
    }
    private void PreviousTarget()
    {
        targetIndex--;
        if (targetIndex < 0)
        {
            targetIndex = possibleTarget.Count-1;
        }
        Select(possibleTarget[targetIndex]);
    }

    public override void OnExit()
    {
        battleManager.actorInfoPanel.Appear(false);
        Time.timeScale = 1;
        base.OnExit();
    }


}
