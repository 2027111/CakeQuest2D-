using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class OverworldTimeline : Timeline
{

    public Character player;
    private void Start()
    {
        CheckPlay();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Character>();
    }

    public override void DialogueRequest()
    {
        DialogueBox.Singleton.StartDialogue(storagePlay.GetLine(), DialogueOver, player.gameObject, gameObject);
    }

    public override void DialogueOver()
    {

        base.DialogueOver();
        player.ChangeState(new PlayerControlsBehaviour());
        
    }



}
