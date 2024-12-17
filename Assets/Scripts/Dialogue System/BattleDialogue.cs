using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Unity.VisualScripting;

[Serializable, Inspectable]
public class BattleDialogue : Dialogue
{
    public BattleCondition technicalCondition;
    public int conditionIndex;
    public bool requiresPrevious = false;
    public bool played = false;

    public bool CheckBattleCondition()
    {

        if (played)
        {
            return false;
        }

        switch (technicalCondition)
        {
            case BattleCondition.None:
                return true;
            case BattleCondition.OnLoop:
                if (BattleManager.Singleton.GetLoopAmount() == conditionIndex && BattleManager.Singleton.IsFirstTurn())
                {
                    return true;
                }
                break;
            case BattleCondition.OnTurn:
                if (BattleManager.Singleton.GetTurnAmount() == conditionIndex && !BattleManager.Singleton.IsEnemyTurn())
                {
                    return true;
                }
                break;
            case BattleCondition.OnEnemyTurn:
                if (BattleManager.Singleton.GetEnemyTurnAmount() == conditionIndex && BattleManager.Singleton.IsEnemyTurn())
                {
                    return true;
                }
                break;

            case BattleCondition.OnObserveEnemy:
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
    public override void SetPlayed()
    {
        played = true;
    }
    public BattleDialogue(Dialogue dialogue) : base(dialogue)
    {
    }
}
