using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleMenu : ChoiceMenu
{


    public void Guard()
    {
        Debug.Log("Guard");
        Command attackCommand = new AttackCommand();
        attackCommand.SetSource(BattleManager.Singleton.GetActor());
        BattleManager.Singleton.GetActor().currentCommand = attackCommand;
        BattleManager.Singleton.ChangeState(new ChoosingTargetState());
    }

    public void Swap()
    {
        BattleManager.Singleton.ChangeState(new SwapingState());
    }
   public void OpenSkillMenu()
    {

        BattleManager.Singleton.ChangeState(new ChoosingSkillState());
    }
    public void OpenItemMenu()
    {
        if (BattleManager.Singleton.GetPlayerItems().Count > 0) { 
            BattleManager.Singleton.ChangeState(new ChoosingItemState());
        }
    }

    public void Attack()
    {
        Command attackCommand = new AttackCommand();
        attackCommand.SetSource(BattleManager.Singleton.GetActor());
        BattleManager.Singleton.GetActor().currentCommand = attackCommand;
        BattleManager.Singleton.ChangeState(new ChoosingTargetState());
    }
}
