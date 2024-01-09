using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu]
[System.Serializable]
public class CharacterObject : ScriptableObject
{


    public string characterName;
    public Sprite[] portraits;


    public int Health;
    public int MaxHealth;
    public int Mana;
    public int MaxMana;

    public int AttackDamage;
    public int ManaDamage;

    public bool isDead;


    public int Armor;
    public int BaseArmor;

    public string HealthName;
    public string ManaName;


    public List<AttackData> AttackList = new List<AttackData>();

    public AnimatorOverrideController animationController;

    public delegate void EventHandler(int health, int maxhealth);
    public EventHandler OnHealthChange;
    public EventHandler OnManaChange;

    public void Revitalize()
    {
        isDead = false;
        Health = MaxHealth;
        Mana = MaxMana;
    }
}
