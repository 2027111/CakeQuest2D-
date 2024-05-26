
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class IActionData : ScriptableObject
{

    public TargetType targetType ;
    public Friendliness friendliness = Friendliness.Friendly;
    public TargetStateType targetStateType = TargetStateType.Alive;
    public Element element = Element.Support;
    public List<GameObject> HitEffect;
    public List<AudioClip> SoundEffect;

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
