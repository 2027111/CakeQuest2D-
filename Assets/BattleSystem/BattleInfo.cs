using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[CreateAssetMenu]
public class BattleInfo : BoolValue
{
    public AudioClip BattleMusic;
    public Cutscene CutsceneForDialogue;
    public GameObject backgroundPrefab;
    public List<CharacterObject> FightParty;




    public void SetInfos(BattleInfo replacement)
    {
        this.CutsceneForDialogue = replacement.CutsceneForDialogue;
        this.BattleMusic = replacement.BattleMusic;
        this.FightParty = replacement.FightParty;
        this.backgroundPrefab = replacement.backgroundPrefab;
    }

    public override void SetRuntime()
    {
        Debug.Log("Set Runtime");
        base.SetRuntime();
    }
}
