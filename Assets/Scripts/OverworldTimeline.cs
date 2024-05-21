using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;

public class OverworldTimeline : Timeline
{

    public Character player;
    private new void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Character>();
        
        base.Start();
    }

    public override void SetupRequirements()
    {

        if (storagePlay.StartRoom != null)
        {

            player.GetComponent<PlayerInfoStorage>().SetNewRoom(storagePlay.StartRoom);
        }
    }

    public override void DialogueRequest()
    {
        Dialogue dialogue = new Dialogue(storagePlay.GetLine());
        dialogue.OnOverEvent.AddListener(DialogueOver);
        Debug.Log("Requesting Dialogue : " + dialogue.OnOverEvent.GetPersistentEventCount());
        DialogueBox.Singleton.StartDialogue(dialogue, player.gameObject, null);
    }



    public override void CutsceneOver()
    {
        base.CutsceneOver();
        player.GetComponent<Character>().ChangeState(new PlayerControlsBehaviour());
    }



}
