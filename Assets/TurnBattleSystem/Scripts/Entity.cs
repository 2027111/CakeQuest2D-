using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    public BattleCharacter lastAttacker;
    BattleCharacter character;
    public int Speed = 50;
    public int Health = 50;
    public int Mana = 50;

    public int Focus = 0;
    public int MaxFocus = 10;
    public delegate void EventHandler(int health, int maxhealth);
    public EventHandler OnHealthChange;
    public EventHandler OnManaChange;
    public EventHandler OnFocusChange;
    public delegate void DamageEventHandler(int amount, ElementEffect elementEffect, BattleCharacter source);
    public delegate void DamageEvent();
    public bool isDead = false;
    public DamageEventHandler OnDamageTaken;
    public DamageEvent OnDead;


    public void AddToMana(int amount)
    {
        if (Mana + amount <= 0)
        {
            Mana = 0;
        }
        else if (Mana + amount >= character.GetReference().MaxMana)
        {

            Mana = character.GetReference().MaxMana;
        }
        else
        {
            Mana += amount;
        }
        if(amount > 0)
        {
            GetComponent<TextEffect>().SpawnTextEffect(amount);
        }
        if (character)
        {
            OnManaChange?.Invoke(Mana, character.GetReference().MaxMana);
        }
    }
    private void Awake()
    {
        character = GetComponent<BattleCharacter>();
    }
    public void ResetHealth()
    {

        Health = character.GetReference().MaxHealth;
        character.GetReference().isDead = false;
    }


    public void AddFocus(int amount)
    {
        Focus = Mathf.Clamp(Focus + amount, 0, MaxFocus);
        OnFocusChange?.Invoke(Focus, MaxFocus);
    }
    public void AddToHealth(int amount)
    {
        if (Health + amount <= 0)
        {

            Health = 0;



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
            if (Health + amount >= character.GetReference().MaxHealth)
            {
                Health = character.GetReference().MaxHealth;
            }
            else
            {
                Health += amount;
            }
            if(amount < 0)
            {
                if (!isDead)
                {
                    character.Animator.Hurt();
                    CamManager.Shake(.2f, .1f);
                    character.StopBlock();
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

                }
            }
        } 
        if (character)
        {
            OnHealthChange?.Invoke(Health, character.GetReference().MaxHealth);
        }
    }
    public void AddToHealth(IActionData attack, ElementEffect effect, BattleCharacter source = null)
    {


        int amount = -source.GetReference().AttackDamage;

        if (attack)
        {
            amount = attack.element == Element.Support?attack.GetAmount():-attack.GetAmount();
        }





        switch (effect)
        {
            case ElementEffect.Neutral:
                // No modification to the damage
                break;

            case ElementEffect.Weak:
                // Increase damage by 50%
                amount = (int)(amount * 1.5f);
                break;

            case ElementEffect.Resistant:
                // Decrease damage by 50%
                amount = (int)(amount * 0.5f);
                break;

            case ElementEffect.NonAffected:
                // No damage taken
                amount = 0;
                break;

            default:
                Debug.LogWarning("Unhandled ElementEffect: " + effect);
                break;
        }



        if (character.isBlocking)
        {
            amount /= 2;
        }


        if (character.isParrying)
        {
            AddFocus((Mathf.Abs(amount) / 2));
            amount = 0;
            character.StopBlock();
            character.Animator.Parry();
            character.PlaySFX(Resources.Load<AudioClip>("39_Block_03"));
            StartCoroutine(Utils.SlowDown(1.1f, .3f));
            //StartCoroutine(CamManager.DoPan(character.transform.position, .05f, 1.1f* .3f));
        }


        if (amount != 0)
        {

            if (attack?.GetHitEffect() != null)
            {
                Instantiate(attack.GetHitEffect(), transform.position + Vector3.up, Quaternion.identity);
            }
            else
            {
                if (source.GetReference().GetHitEffect() != null)
                {
                    Instantiate(source.GetReference().GetHitEffect(), transform.position + Vector3.up, Quaternion.identity);
                }
            }
            if (attack?.GetSoundEffect() != null)
            {
                source.PlaySFX(attack.GetSoundEffect());
            }
            else
            {
                if (source.GetReference().GetSoundEffect() != null)
                {
                    source.PlaySFX(source.GetReference().GetSoundEffect());
                }
            }

        }
        AddToHealth(amount);

        // Invoke the damage taken event
        OnDamageTaken.Invoke(amount, effect, source);
    }

    public void AddToHealth(HealthEffectItem item, ElementEffect effect, BattleCharacter source = null)
    {


        int amount = 0;
        if (item)
        {
            amount = item.element == Element.Support ? item.healthEffectAmount : -item.healthEffectAmount;
        }







        if (character.isBlocking)
        {
            amount /= 2;
        }


        if (character.isParrying)
        {
            AddFocus((Mathf.Abs(amount) / 2));
            amount = 0;
            character.StopBlock();
            character.Animator.Parry();
            character.PlaySFX(Resources.Load<AudioClip>("39_Block_03"));
            StartCoroutine(Utils.SlowDown(1.1f, .3f));
            //StartCoroutine(CamManager.DoPan(character.transform.position, .05f, 1.1f* .3f));
        }


        if (amount != 0)
        {

            if (item?.GetHitEffect() != null)
            {
                Instantiate(item.GetHitEffect(), transform.position + Vector3.up, Quaternion.identity);
            }
            else
            {
                if (source.GetReference().GetHitEffect() != null)
                {
                    Instantiate(source.GetReference().GetHitEffect(), transform.position + Vector3.up, Quaternion.identity);
                }
            }
            if (item?.GetSoundEffect() != null)
            {
                source.PlaySFX(item.GetSoundEffect());
            }
            else
            {
                if (source.GetReference().GetSoundEffect() != null)
                {
                    source.PlaySFX(source.GetReference().GetSoundEffect());
                }
            }

        }
        AddToHealth(amount);

        // Invoke the damage taken event
        OnDamageTaken.Invoke(amount, effect, source);
    }

    public void Apply()
    {
        character.GetReference().Health = Health;
        character.GetReference().Mana = Mana;
        character.GetReference().isDead = isDead;
    }

    public void LoadReference()
    {
        Health = character.GetReference().Health;
        Mana =character.GetReference().Mana;
        isDead = character.GetReference().isDead;

        if (isDead)
        {
            character.Animator.Die();
        }



        AddToHealth(0);
        AddToMana(0);
    }

    public void LoadReferenceRefreshed()
    {
        Health = character.GetReference().MaxHealth;
        Mana = character.GetReference().MaxMana;
        isDead = false;



        AddToHealth(0);
        AddToMana(0);
    }
}
