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

    public override void SetupRequirements()
    {

        if (storagePlay.StartRoom != null)
        {
            PlayerInfoStorage.CurrentInfoStorage.SetNewRoom(storagePlay.StartRoom);
            
        }

        playableDirector.playableAsset = storagePlay.CutsceneToPlay;
    }

    public override void DialogueRequest()
    {
        Dialogue dialogue = storagePlay.GetNextLine();
        dialogue.OnOverEvent.AddListener(DialogueOver);
        UICanvas.StartDialogue(dialogue, Character.Player.gameObject, null);
    }



    public override void CutsceneOver()
    {
        base.CutsceneOver();


        if (storagePlay.EndRoom != null)
        {
            PlayerInfoStorage.CurrentInfoStorage.SetNewRoom(storagePlay.EndRoom, true);
        }
        Character.Player.GetComponent<Character>().ChangeState(new PlayerControlsBehaviour());
        OnCutsceneOver?.Invoke();
    }



}
