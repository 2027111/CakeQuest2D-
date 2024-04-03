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
        Debug.Log("Requesting Dialogue");
        Dialogue dialogue = new Dialogue(storagePlay.GetLine());
        dialogue.OnOverEvent.AddListener(DialogueOver);
        DialogueBox.Singleton.StartDialogue(dialogue, null, null, GameState.BattleScene);
    }


    public override void DialogueOver()
    {
        base.DialogueOver();
    }

    public override void CutsceneOver()
    {
        base.CutsceneOver();
        battleManager.StartBattle();
    }

}

