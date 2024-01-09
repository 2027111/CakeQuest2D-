using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class BattleTimeline : Timeline
{
    public BattleManager battleManager;

    public override void DialogueRequest()
    {
        DialogueBox.Singleton.StartDialogue(storagePlay.GetLine(), DialogueOver, null, null, GameState.BattleScene);
    }

    public override void DialogueOver()
    {
        base.DialogueOver();
        battleManager.StartBattle();
    }

}

