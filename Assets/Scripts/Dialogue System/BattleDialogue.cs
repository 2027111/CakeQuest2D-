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

  
    public BattleDialogue(BattleDialogueHolder battleDialogueHolder) : base(battleDialogueHolder.dialogue, battleDialogueHolder.DialogueEvents)
    {
        OnOverEvent.AddListener(battleDialogueHolder.SetPlayed);
    }
}
