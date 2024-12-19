using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


[Serializable]
public class BattleCondition
{

    public BattleConditionType technicalCondition;
    public int conditionIndex;
    public bool requiresPrevious = false;
    public bool played = false;

    public BattleCondition(BattleCondition battleConditionParam)
    {
        this.technicalCondition = battleConditionParam.technicalCondition;
        this.conditionIndex = battleConditionParam.conditionIndex;
        this.requiresPrevious = battleConditionParam.requiresPrevious;
    }
    public BattleCondition()
    {
    }



    public bool CheckBattleCondition()
    {

        if (played)
        {
            return false;
        }

        switch (technicalCondition)
        {
            case BattleConditionType.None:
                return true;
            case BattleConditionType.OnLoop:
                if (BattleManager.Singleton.GetLoopAmount() == conditionIndex && BattleManager.Singleton.IsFirstTurn())
                {
                    return true;
                }
                break;
            case BattleConditionType.OnTurn:
                if (BattleManager.Singleton.GetTurnAmount() == conditionIndex && !BattleManager.Singleton.IsEnemyTurn())
                {
                    return true;
                }
                break;
            case BattleConditionType.OnEnemyTurn:
                if (BattleManager.Singleton.GetEnemyTurnAmount() == conditionIndex && BattleManager.Singleton.IsEnemyTurn())
                {
                    return true;
                }
                break;

            case BattleConditionType.OnObserveEnemy:
                if (BattleManager.Singleton.isObserving)
                {
                    if (BattleManager.Singleton.ObservationTarget() == conditionIndex)
                    {
                        return true;
                    }
                }
                break;
            default:
                break;
        }
        return false;
    }
}

