using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChoosingTargetState : BattleState
{
    public bool multiple = false;
    public List<BattleCharacter> target = new List<BattleCharacter>();
    public List<BattleCharacter> possibleTarget;
    int targetIndex = 0;
    public override void OnEnter(BattleManager _battleManager)
    {
        base.OnEnter(_battleManager);

        battleManager.SetCursor(null);
        if (battleManager.GetActor().currentCommand == null)
        {
            battleManager.ChangeState(new ChoosingActionState());
        }
        else
        {

            if (battleManager.GetActor().currentCommand is SkillCommand)
            {
                TargetType tt = ((SkillCommand)(battleManager.GetActor().currentCommand)).GetAttack().targetType;
                if (tt == TargetType.Single)
                {
                    multiple = false;

                }
                else if (tt == TargetType.Full)
                {

                    multiple = true;
                }




            }
            possibleTarget = battleManager.GetPossibleTarget();
            if (possibleTarget.Count == 0)
            {
                battleManager.ChangeState(new ChoosingActionState());
            }
            else
            {

                if (multiple)
                {

                    target = possibleTarget;
                    foreach (BattleCharacter bc in target)
                    {

                        if (bc != null)
                        {
                            Select(bc);
                        }
                    }
                }
                else
                {
                    Select(possibleTarget[targetIndex]);
                }
            }
        }

    }

    public void Select(BattleCharacter character)
    {
            if (!multiple)
            {
                target.Clear();
                target.Add(character);
                battleManager.SetCursor(character);
                CamManager.PanToCharacter(character);
                BattleManager.Singleton.SetIndicationText("Target " + character.name);
            }
            else
            {
                battleManager.SetCursor(character, false);
                CamManager.ResetView();

            BattleManager.Singleton.SetIndicationText("Target everyone");
        }
    }


    public override void Handle()
    {
        base.Handle();
    }
    public override void OnSelect()
    {
        battleManager.GetActor().currentCommand.SetTarget(target);
        battleManager.ChangeState(new PerformActionState());
        base.OnSelect();
    }

    public override void OnBack()
    {
        if (battleManager.GetActor().currentCommand is SkillCommand)
        {
            battleManager.ChangeState(new ChoosingSkillState());
        }
        else if (battleManager.GetActor().currentCommand.GetType() == typeof(AttackCommand))
        {
            battleManager.ChangeState(new ChoosingActionState());
        }
        else if (battleManager.GetActor().currentCommand.GetType() == typeof(ItemCommand))
        {
            battleManager.ChangeState(new ChoosingItemState());
        }
        base.OnBack();
    }
    public override void ShowControls()
    {
        battleManager.SetControlText("WASD to change target | Left Click to confirm target(s) | Right Click to return");
    }
    public override void OnNavigate(Vector2 direction)
    {
        if (!multiple)
        {
            if (direction.x > 0)
            {
                NextTarget();
            }
            else if (direction.x < 0)
            {

                PreviousTarget();
            }

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
        base.OnExit();
    }


}
