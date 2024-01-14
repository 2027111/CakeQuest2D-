using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class Timeline: MonoBehaviour
{
    public Cutscene storagePlay;
    public bool started = false;
    public PlayableDirector playableDirector;
    private void Start()
    {

        CheckPlay();
    }


    public virtual void StartDialogue()
    {
        if (!started)
        {
            if(storagePlay.GetCurrentLine() != null)
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


    public void SetCutscene(Cutscene cutscene)
    {
        storagePlay = cutscene;
    }
    public virtual void DialogueRequest()
    {

        DialogueBox.Singleton.StartDialogue(storagePlay.GetLine(), DialogueOver, null, null);
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

    public void CutsceneOver()
    {
        storagePlay.dialogueIndex = 0;
        if (storagePlay)
        {
            if (!storagePlay.repeats)
            {

                storagePlay.RuntimeValue = true;
            }
            CheckPlay();
        }
    }
    public void CheckPlay()
    {
        
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
