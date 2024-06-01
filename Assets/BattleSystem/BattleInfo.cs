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




    public void SetInfos(BattleInfo replacement)
    {
        this.CutsceneForDialogue = replacement.CutsceneForDialogue;
        this.BattleMusic = replacement.BattleMusic;
    }
}
