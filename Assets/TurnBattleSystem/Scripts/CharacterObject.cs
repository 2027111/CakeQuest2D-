
using Newtonsoft.Json;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu]
[System.Serializable]
public class CharacterObject : ScriptableObject
{


   [JsonIgnore] public CharacterData characterData;
    [Space(20)]
    public int Health;
    public int MaxHealth;
    [Space(20)]
    public int Mana;
    public int MaxMana;
    [Space(20)]
    public int Speed;
    public int AttackDamage;
    public float parryWindow = .15f;

    [Space(20)]
    public bool isDead;

    [Space(20)]
    public SkillType attackType = SkillType.Physical;
    [Space(20)]
    public Element AttackElement;
    [Space(20)]

    [JsonIgnore] public List<ElementalAttribute> elementalAttributes;
    [JsonIgnore] public List<GameObject> HitEffect;
    [JsonIgnore] public List<AudioClip> SoundEffect;

    [Space(40)]
    public List<Skill> Attacks;

    [Space(20)]
    [JsonIgnore] public AnimatorOverrideController animationController;


    public void Revitalize()
    {
        isDead = false;
        Health = MaxHealth;
        Mana = MaxMana;

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

    public ElementEffect GetElementEffect(Element element)
    {
        foreach (ElementalAttribute ea in elementalAttributes)
        {
            if (ea.element == element)
            {
                return ea.elementEffect;
            }
        }
        return ElementEffect.Neutral;
    }

    public float GetParryWindowTime()
    {
        return parryWindow;
    }
}
