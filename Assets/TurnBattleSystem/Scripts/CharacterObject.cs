using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu]
[System.Serializable]
public class CharacterObject : ScriptableObject
{


    public CharacterData characterData;
    [Space(10)]
    public int Health;
    public int MaxHealth;
    [Space(10)]
    public int Mana;
    public int MaxMana;
    [Space(20)]
    public int Speed;
    public int AttackDamage;
    public int ManaDamage;

    [Space(10)]
    public bool isDead;


    public List<Attack> Attacks;

    public AnimatorOverrideController animationController;


    public void Revitalize()
    {
        isDead = false;
        Health = MaxHealth;
        Mana = MaxMana;

    }

}
