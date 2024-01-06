using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class WrittenTimeline : MonoBehaviour
{
    public BoolValue storagePlay;
    public LineInfo[] dialogueLines;
    public bool started = false;
    public PlayableDirector playableDirector;
    public Character player;
    private void Start()
    {
        CheckPlay();


        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Character>();
    }


    public void StartDialogue()
    {
        if (!started)
        {
            started = true;
            DialogueBox.Singleton.StartDialogue(dialogueLines, DialogueOver, player.gameObject, gameObject);
        }
    }


    public void DialogueOver()
    {
        started = false;
        Debug.Log("Dialogue Over");
        player.ChangeState(new PlayerControlsBehaviour());
        UnpauseCutscene();
    }

    public void UnpauseCutscene()
    {
        playableDirector.Play();
        Debug.Log("Unpause");
    }

    public void StopCutsceneLoop()
    {
        storagePlay.initialValue = true;
        CheckPlay();
    }
    public void CheckPlay()
    {
        if (storagePlay.initialValue)
        {
            DeleteCutscene();
        }
    }

    public void DeleteCutscene()
    {
        Destroy(gameObject);
    }
}
