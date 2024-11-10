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
        Dialogue ddialogue = storagePlay.GetNextLine();
        Dialogue dialogue = new Dialogue(ddialogue);
        Debug.Log(ddialogue == null);
        dialogue.OnOverEvent.AddListener(ddialogue.SetPlayed);
        dialogue.OnOverEvent.AddListener(storagePlay.SetRuntime);
        dialogue.OnOverEvent.AddListener(DialogueOver);
        UICanvas.StartDialogue(dialogue, null, null, GameState.BattleScene);
    }


    public override void DialogueOver()
    {

        started = false;
        battleManager.StartNewTurn();

    }


    public override void StartDialogue()
    {
        if (!started)
        {
            if (CanPlayCutscene())
            {
                if (storagePlay.GetCurrentLine() != null)
                {

                    started = true;
                    DialogueRequest();
                }
                else
                {
                    DialogueOver();
                }
            }
        }
    }



    public override void CutsceneOver()
    {
        base.CutsceneOver();

    }

    public override void StartCinematic()
    {
        if (CanPlayCutscene())
        {
            storagePlay.dialogueIndex = 0;
            BattleManager.Singleton.StartingDialogue();

            IsInCutscene = true;
        }
    }

    public bool HasCutscene()
    {
        if (CanPlayCutscene())
        {
            if (storagePlay)
            {
                if (storagePlay is BattleCutscene)
                {
                    if (((BattleCutscene)storagePlay).GetPlayableLine() != null)
                    {
                        return true;
                    }
                }
            }

        }
        return false;
    }

    public void ResetPlayed()
    {
        if (storagePlay)
        {
            storagePlay.ResetPlayed();
        }

    }
}

