using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleMenu : MonoBehaviour
{


    public void Guard()
    {
        Debug.Log("Guard");
        Command attackCommand = new AttackCommand();
        attackCommand.SetSource(BattleManager.Singleton.GetActor());
        BattleManager.Singleton.GetActor().currentCommand = attackCommand;
        BattleManager.Singleton.ChangeState(new ChoosingTargetState());
    }

    public void Heal()
    {
        Debug.Log("Heal");
        //Command attackCommand = new DefensiveCommand();
        //attackCommand.SetSource(BattleManager.Singleton.GetActor());
        //BattleManager.Singleton.GetActor().currentCommand = attackCommand;
        //BattleManager.Singleton.ChangeState(new ChoosingTargetState());
    }
   public void OpenSkillMenu()
    {

        BattleManager.Singleton.ChangeState(new ChoosingSkillState());
    }

    public void Attack()
    {
        Command attackCommand = new AttackCommand();
        attackCommand.SetSource(BattleManager.Singleton.GetActor());
        BattleManager.Singleton.GetActor().currentCommand = attackCommand;
        BattleManager.Singleton.ChangeState(new ChoosingTargetState());
    }
}
