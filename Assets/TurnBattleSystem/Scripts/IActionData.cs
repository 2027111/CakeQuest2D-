
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif


public class IActionData : ScriptableObject
{

    public string UID;
    private void OnValidate()
    {
        #if UNITY_EDITOR
        if (UID == "")
        {
            UID = GUID.Generate().ToString();
            UnityEditor.EditorUtility.SetDirty(this);
        }
        #endif
    }


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
