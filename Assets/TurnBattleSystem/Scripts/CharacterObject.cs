using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu]
[System.Serializable]
public class CharacterObject : ScriptableObject
{


    public CharacterData characterData;
    [Space(20)]
    public int Health;
    public int MaxHealth;
    [Space(20)]
    public int Mana;
    public int MaxMana;
    [Space(20)]
    public int Speed;
    public int AttackDamage;
    public int ManaDamage;
    public float parryWindow = .15f;

    [Space(20)]
    public bool isDead;

    [Space(20)]
    public Element AttackElement;
    [Space(20)]

    public List<ElementalAttribute> elementalAttributes;

    [Space(40)]
    public List<Attack> Attacks;

    [Space(20)]
    public AnimatorOverrideController animationController;


    public void Revitalize()
    {
        isDead = false;
        Health = MaxHealth;
        Mana = MaxMana;

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
