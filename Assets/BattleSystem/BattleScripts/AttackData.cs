using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;







[CreateAssetMenu(fileName = "New AttackData", menuName = "AttackData")]
[Serializable]
public class AttackData : MoveData
{
    public string animationName;
    public MoveData nextMovePart;
    public AnimationClip animation;
    public float durationInFrames;
    public int startOpenFrame;
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
