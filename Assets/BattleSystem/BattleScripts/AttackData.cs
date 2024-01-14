using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;







public enum AttackProperties
{
    None,
    FriendlyTarget,
    EnemyTarget
}
[CreateAssetMenu(fileName = "New AttackData", menuName = "AttackData")]
[Serializable]
public class AttackData : ScriptableObject
{
    public bool conserveVelocity;
    public string animationName;
    public AnimationClip animation;
    public AttackPlacement attackPlacement = AttackPlacement.NONE;
    public float durationInFrames;
    public int startOpenFrame;
    public int manaCost;
    public GameObject HitEffect;
    public AudioClip SoundEffect;
    public AudioClip VoiceLine;
    public List<HitBoxInfo> hitboxes;
    public List<PrefabInfo> prefabs;
    public List<ForceEvents> forceEvents;

    public HitBoxInfo GetHitBoxByFrame(int frame)
    {
        foreach(HitBoxInfo hitBoxInfo in hitboxes)
        {
            if (hitBoxInfo.frame + hitBoxInfo.durationInFrame > frame  && hitBoxInfo.frame <= frame)
            {
                return hitBoxInfo;
            } 
        }
        return null;
    }

    public void SpawnHitEffect(Vector3 position)
    {
        if (HitEffect)
        {
        Destroy(Instantiate(HitEffect, position, Quaternion.identity), 2f);

        }
    }


    public AudioClip GetVoiceLine()
    {
        return VoiceLine;
    }
    public AudioClip GetSoundEffect()
    {
        return SoundEffect;
    }



    public ForceEvents GetForceEventsByFrame(int frame)
    {
        foreach (ForceEvents forceEvent in forceEvents)
        {
            if (forceEvent.onFrame == frame)
            {
                return forceEvent;
            }
        }
        return null;
    }

    public PrefabInfo GetPrefabByFrame(int frame)
    {
        foreach (PrefabInfo prefab in prefabs)
        {
            if (prefab.frame == frame)
            {
                return prefab;
            }
        }
        return null;
    }
}
