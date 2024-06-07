
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;


public class IActionData : SavableObject
{



    public TargetType targetType ;
    public Friendliness friendliness = Friendliness.Friendly;
    public TargetStateType targetStateType = TargetStateType.Alive;
    public Element element = Element.Support;
    [JsonIgnore] public List<GameObject> HitEffect;
    [JsonIgnore] public List<AudioClip> SoundEffect;





    public override void ApplyData(SavableObject tempCopy)
    {
        base.ApplyData(tempCopy);
    }


    public GameObject GetHitEffect()
    {
        if (HitEffect.Count > 0)
        {
            return HitEffect[Random.Range(0, HitEffect.Count)];
        }
        return null;

    }

    public AudioClip GetSoundEffect()
    {
        if (SoundEffect.Count > 0)
        {
            return SoundEffect[Random.Range(0, SoundEffect.Count)];
        }
        return null;

    }

    public virtual int GetAmount()
    {
        return 0;
    }

}