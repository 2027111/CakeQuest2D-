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
        Dialogue ddialogue = storagePlay.GetLine();
        Dialogue dialogue = new Dialogue(ddialogue);
        dialogue.OnOverEvent.AddListener(ddialogue.SetPlayed);
        dialogue.OnOverEvent.AddListener(storagePlay.SetRuntime);
        dialogue.OnOverEvent.AddListener(DialogueOver);
        DialogueBox.Singleton.StartDialogue(dialogue, null, null, GameState.BattleScene);
    }


    public override void DialogueOver()
    {

        Debug.Log("Battle Cutscene is Over");
        started = false;
        battleManager.StartNewTurn();

    }


    public override void StartDialogue()
    {
        Debug.Log("Starting Dialogue");
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

            SetupRequirements();

            BattleManager.Singleton.StartingDialogue();
        }
    }

    public bool HasCutscene()
    {
        Debug.Log("Checking for cutscene : ");
        if (CanPlayCutscene())
        {
            if (storagePlay)
            {
                if(storagePlay is BattleCutscene)
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

