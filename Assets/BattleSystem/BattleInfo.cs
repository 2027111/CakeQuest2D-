using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[CreateAssetMenu]
public class BattleInfo : SavableObject
{
    public AudioClip BattleMusic;
    public TimelineAsset CutsceneToPlay;
    public Cutscene CutsceneForDialogue;




    public void SetInfos(BattleInfo replacement)
    {
        this.CutsceneToPlay = replacement.CutsceneToPlay;
        this.CutsceneForDialogue = replacement.CutsceneForDialogue;
        this.BattleMusic = replacement.BattleMusic;
    }
}
