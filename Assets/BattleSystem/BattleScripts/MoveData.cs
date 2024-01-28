using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;






public enum AttackType
{
    Normal,
    Special
}

[CreateAssetMenu(fileName = "New AttackData", menuName = "MoveData")]
[Serializable]
public class MoveData : ScriptableObject
{
    public string MoveName = "Attack";
    public bool conserveVelocity;
    public bool grounded;
    public AttackPlacement attackPlacement = AttackPlacement.NONE;
    public AttackType attackType = AttackType.Normal;
    public int manaCost;
    public List<GameObject> HitEffect = new List<GameObject>();
    public List<AudioClip> SoundEffect = new List<AudioClip>();
    public List<AudioClip> VoiceLine = new List<AudioClip>();


    public void SpawnHitEffect(Vector3 position)
    {
        if (HitEffect.Count > 0)
        {
            Destroy(Instantiate(HitEffect[UnityEngine.Random.Range(0, HitEffect.Count-1)], position, Quaternion.identity), 2f);

        }
    }


    public AudioClip GetVoiceLine()
    {
        if (VoiceLine.Count > 0)
        {
            return VoiceLine[UnityEngine.Random.Range(0, VoiceLine.Count - 1)]; ;
        }
        else
        {
            return null;
        }
    }
    public AudioClip GetSoundEffect()
    {
        if (SoundEffect.Count > 0)
        {
            return SoundEffect[UnityEngine.Random.Range(0, SoundEffect.Count - 1)];
        }
        else
        {
            return null;
        }
    }



}
