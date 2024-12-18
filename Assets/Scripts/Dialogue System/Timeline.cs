using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;

public class Timeline : MonoBehaviour
{

    public static bool IsInCutscene = false;

    public double currentLoopPointStart;
    public double currentLoopPointEnd;
    public bool loopingSection = false;

    public ConditionResultObject[] condition;
    public Cutscene storagePlay;
    public bool started = false;
    public PlayableDirector playableDirector;
    public UnityEvent OnCutsceneOver;
    public bool Automatic;
    protected void Start()
    {
        if (Automatic)
        {
            if (FadeScreen.fading || FadeScreen.fadeOn)
            {

                StartCinematic(true);
            }
            else
            {
                StartCinematic();
            }
           
        }
    }

    public virtual void StartCinematic(bool delayed = false)
    {
        if (CanPlayCutscene())
        {
            // Debug.Log("Playing Cutscene");
            storagePlay.ResetPlayed();
            Character.Player?.ToggleCutsceneState();
            SetupRequirements();
            if (delayed)
            {

                StartLoopSection(.001f);
                FadeScreen.AddOnEndFadeEvent(StopLoopSection);
            }
            playableDirector.Play();
            IsInCutscene = true;
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
        //Debug.Log("Starting Dialogue");
        if (!started)
        {
            if (CanPlayCutscene())
            {
                if (storagePlay.GetCurrentLine() != null)
                {

                    started = true;

                    StartLoopSection(.1f);
                    //playableDirector.Pause();
                    DialogueRequest();
                }
                else
                {
                    DialogueOver();
                }
            }
        }
    }

    private void Update()
    {
        if (loopingSection)
        {
            if (playableDirector != null && playableDirector.time >= currentLoopPointEnd)
            {
                playableDirector.time = currentLoopPointStart;
                playableDirector.Evaluate();
                playableDirector.Play();
            }
        }
    }

    public virtual void StartDialogue(float loopTime)
    {
        //Debug.Log("Starting Dialogue");
        if (!started)
        {
            if (CanPlayCutscene())
            {
                if (storagePlay.GetCurrentLine() != null)
                {

                    started = true;
                    StartLoopSection(loopTime);
                    //playableDirector.Pause();
                    DialogueRequest();
                }
                else
                {
                    DialogueOver();
                }
            }
        }
    }
    public void StartLoopSection(float loopTime)
    {
        currentLoopPointStart = playableDirector.time + .01f;
        currentLoopPointEnd = playableDirector.time + .01f + loopTime;
        loopingSection = true;
       
    }

    public void StopLoopSection()
    {
        loopingSection = false;
    }

    public void SetCutscene(Cutscene cutscene)
    {
        storagePlay = cutscene;
    }
    public virtual void DialogueRequest()
    {
        Dialogue dialogue = new Dialogue(storagePlay.GetNextLine());
        dialogue.OnOverEvent.AddListener(DialogueOver);
        //Debug.Log("Requesting Dialogue : " + dialogue.OnOverEvent.GetNonPersistentEventCount());
        UICanvas.StartDialogue(dialogue, null, null);
    }

    public virtual void DialogueOver()
    {
        started = false;
        //Debug.Log("Dialogue Over");
        StopLoopSection();
       // UnpauseCutscene();
    }

    public void UnpauseCutscene()
    {
        playableDirector?.Resume();
       // Debug.Log("Unpause");
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

        IsInCutscene = false;
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

    public bool CheckCondition()
    {
        foreach (ConditionResultObject c in condition)
        {
            if (!c.CheckCondition())
            {
                return false;
            }
        }
        return true;
    }
    public bool CanPlayCutscene()
    {
        if (!CheckCondition())
        {
            return false;
        }
        if (storagePlay)
        {
            return !storagePlay.RuntimeValue;
        }
        return false;
    }

    public void FadeTo()
    {
        FadeScreen.SetColor(Color.white);
        StartCoroutine(FadeScreen.Singleton.StartFadeAnimation(true, .1f));
    }


    public void Flash()
    {
        FadeScreen.SetColor(Color.white);
        StartCoroutine(FadeScreen.Singleton.StartFlashAnimation(.1f));
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
