using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;

public class Timeline : MonoBehaviour
{

    public BoolValue condition;
    public Cutscene storagePlay;
    public bool started = false;
    public PlayableDirector playableDirector;
    public UnityEvent OnCutsceneOver;
    public bool Automatic;
    protected void Start()
    {
        if (Automatic)
        {
            StartCinematic();
        }
    }

    public virtual void StartCinematic()
    {
        if (CanPlayCutscene())
        {
            storagePlay.dialogueIndex = 0;

            SetupRequirements();

            playableDirector.Play();
        }

    }

    public virtual void SetupRequirements()
    {
        playableDirector.playableAsset = storagePlay.CutsceneToPlay;
    }


    public void PlaySong(string songName)
    {

        MusicPlayer.Singleton.PlaySong(songName);
    }

    public virtual void StartDialogue()
    {
        Debug.Log("Starting Dialogue");
        if (!started)
        {
            if (CanPlayCutscene())
            {
                if (storagePlay.GetCurrentLine() != null)
                {

                    started = true;
                    playableDirector.Pause();
                    DialogueRequest();
                }
                else
                {
                    DialogueOver();
                }
            }
        }
    }


    public void SetCutscene(Cutscene cutscene)
    {
        storagePlay = cutscene;
    }
    public virtual void DialogueRequest()
    {
        Dialogue dialogue = new Dialogue(storagePlay.GetLine());
        dialogue.OnOverEvent.AddListener(DialogueOver);
        //Debug.Log("Requesting Dialogue : " + dialogue.OnOverEvent.GetNonPersistentEventCount());
        UICanvas.StartDialogue(dialogue, null, null);
    }

    public virtual void DialogueOver()
    {
        started = false;
        Debug.Log("Dialogue Over");
        UnpauseCutscene();
    }

    public void UnpauseCutscene()
    {
        playableDirector?.Resume();
        Debug.Log("Unpause");
    }

    public virtual void CutsceneOver()
    {
        storagePlay.dialogueIndex = 0;
        if (storagePlay)
        {
            if (!storagePlay.repeats)
            {

                storagePlay.RuntimeValue = true;
            }
        }
    }
    public void StopReplay()
    {
        if (storagePlay)
        {
            if (!storagePlay.repeats)
            {

                storagePlay.RuntimeValue = true;
            }
        }
    }
    public bool CanPlayCutscene()
    {
        if (condition)
        {
            if (!condition.RuntimeValue)
            {
                return false;
            }
        }
        if (storagePlay)
        {
            return !storagePlay.RuntimeValue;
        }
        return false;
    }

    public void FadeTo() { 
    }
    public void PlayCutscene()
    {
        playableDirector.Play();
    }
    public void DeleteCutscene()
    {
        Destroy(gameObject);
    }
}
