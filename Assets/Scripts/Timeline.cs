using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class Timeline: MonoBehaviour
{
    public Cutscene storagePlay;
    public bool started = false;
    public PlayableDirector playableDirector;
    public bool Automatic;
    protected void Start()
    {
        if (Automatic)
        {
            StartCinematic();
        }
    }

    public void StartCinematic()
    {
        if (CheckPlay())
        {
            storagePlay.dialogueIndex = 0;
            playableDirector.Play();
        }

    }

    public virtual void StartDialogue()
    {
        Debug.Log("Starting Dialogue");
        if (!started )
        {
            if (CheckPlay())
            {
                if (storagePlay.GetCurrentLine() != null)
                {

                    started = true;
                    playableDirector.Pause();
                    DialogueRequest();
                }
                else
                {
                    CutsceneOver();
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
        DialogueBox.Singleton.StartDialogue(dialogue, null, null);
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
    public bool CheckPlay()
    {
        if (storagePlay)
        {
            return !storagePlay.RuntimeValue;
        }
        return false;
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
