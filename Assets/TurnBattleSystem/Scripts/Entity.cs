using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    public BattleCharacter lastAttacker;
    BattleCharacter character;
    public delegate void DamageEventHandler(int amount, BattleCharacter source);
    public delegate void DamageEvent();
    public bool isDead = false;
    public DamageEventHandler OnDamageTaken;
    public DamageEvent OnDead;


    public void AddToMana(int amount)
    {
        if (character.Mana + amount <= 0)
        {
            character.Mana = 0;
        }
        else if (character.Mana + amount >= character.GetReference().MaxMana)
        {

            character.Mana = character.GetReference().MaxMana;
        }
        else
        {
            character.Mana += amount;
        }
        if (character)
        {
            character.OnManaChange?.Invoke(character.Mana, character.GetReference().MaxMana);
        }
    }
    private void Awake()
    {
        character = GetComponent<BattleCharacter>();
    }
    public void ResetHealth()
    {

        character.Health = character.GetReference().MaxHealth;
        character.GetReference().isDead = false;
    }

    public void AddToHealth(int amount)
    {
        if (character.Health + amount <= 0)
        {

            character.Health = 0;



            if (!isDead)
            {
                isDead = true;
                OnDead?.Invoke();
                character.Animator.Die();
                CamManager.Shake(.2f, .1f);
            }



        }
        else
        {
            if (character.Health + amount >= character.GetReference().MaxHealth)
            {
                character.Health = character.GetReference().MaxHealth;
            }
            else
            {
                character.Health += amount;
            }
            if(amount < 0)
            {
                if (!isDead)
                {
                    character.Animator.Hurt();
                    CamManager.Shake(.2f, .1f);
                }
            }
            else if(amount > 0)
            {
                if (isDead)
                {
                    isDead = false;
                    character.Animator.Revive();
                }
                else
                {
                    character.Animator.Dodge();
                }
            }
        } 
        if (character)
        {
            character.OnHealthChange?.Invoke(character.Health, character.GetReference().MaxHealth);
        }
    }

    public void TakeDamage(int amount, BattleCharacter attacker = null)
    {
        lastAttacker = attacker;
        if (character.Health + amount >= 0)
        {
            AddToHealth(amount);

        }
        else
        {
            AddToHealth(-character.Health);
        }
        OnDamageTaken.Invoke(amount, attacker);
        lastAttacker = null;

    }
    public void Apply()
    {
        character.GetReference().Health = character.Health;
        character.GetReference().Mana = character.Mana;
        character.GetReference().isDead = isDead;
    }

    public void LoadReference()
    {
        character.Health = character.GetReference().Health;
        character.Mana =character.GetReference().Mana;
        isDead = character.GetReference().isDead;

        if (isDead)
        {
            character.Animator.Die();
        }



        AddToHealth(0);
        AddToMana(0);
    }
}
