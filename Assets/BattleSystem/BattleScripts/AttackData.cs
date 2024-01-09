using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public enum AttackType
{
    Melee,
    Projectile,
    Spell,
    TargetSpell,
}


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
    public AttackType attackType = AttackType.Melee;
    public AttackProperties attackProperties = AttackProperties.None;
    public AttackPlacement attackPlacement = AttackPlacement.NONE;
    public float durationInFrames;
    public int startOpenFrame;
    public int manaCost;
    public List<HitBoxInfo> hitboxes;
    public List<PrefabInfo> prefabs;
    public List<ForceEvents> forceEvents;

    public HitBoxInfo GetHitBoxByFrame(int frame)
    {
        foreach(HitBoxInfo hitBoxInfo in hitboxes)
        {
            if (hitBoxInfo.frame >= frame-durationInFrames && hitBoxInfo.frame <= frame)
            {
                return hitBoxInfo;
            } 
        }
        return null;
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

    internal PrefabInfo GetPrefabByFrame(int frame)
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
