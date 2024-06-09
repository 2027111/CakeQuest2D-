using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[CreateAssetMenu]
public class BattleInfo : BoolValue
{
    [JsonIgnore] public AudioClip BattleMusic;
    [JsonIgnore] public Cutscene CutsceneForDialogue;
    [JsonIgnore] public GameObject backgroundPrefab;
    [JsonIgnore] public List<CharacterObject> FightParty;




    public void SetInfos(BattleInfo replacement)
    {
        this.CutsceneForDialogue = replacement.CutsceneForDialogue;
        this.BattleMusic = replacement.BattleMusic;
        this.FightParty = replacement.FightParty;
        this.backgroundPrefab = replacement.backgroundPrefab;
    }
}
