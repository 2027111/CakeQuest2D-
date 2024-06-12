using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;

public class OverworldTimeline : Timeline
{


    private new void Start()
    {
        
        base.Start();
    }
    public override void StartCinematic()
    {
        base.StartCinematic();
    }
    public override void SetupRequirements()
    {

        if (storagePlay.StartRoom != null)
        {

            Character.Player.GetComponent<PlayerInfoStorage>().SetNewRoom(storagePlay.StartRoom);
        }

        playableDirector.playableAsset = storagePlay.CutsceneToPlay;
    }

    public override void DialogueRequest()
    {
        Dialogue dialogue = new Dialogue(storagePlay.GetLine());
        dialogue.OnOverEvent.AddListener(DialogueOver);
        UICanvas.StartDialogue(dialogue, Character.Player.gameObject, null);
    }



    public override void CutsceneOver()
    {
        base.CutsceneOver();
        Character.Player.GetComponent<Character>().ChangeState(new PlayerControlsBehaviour());
        OnCutsceneOver?.Invoke();
    }



}
