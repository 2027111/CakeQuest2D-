using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    public Rigidbody2D rb;
    public StateMachine stateMachine;
    public CharacterObject characterObject;
    public Entity lastAttacker;

    public delegate void DamageEventHandler(int amount, BattleCharacter source);
    public delegate void DamageEvent();

    public DamageEventHandler OnDamageTaken;
    public DamageEvent OnDead;

    public float damageChain = 0;
    public void AddToMana(int amount)
    {
        if (characterObject.Mana + amount <= 0)
        {
            characterObject.Mana = 0;
        }
        else if (characterObject.Mana + amount >= characterObject.MaxMana)
        {

            characterObject.Mana = characterObject.MaxMana;
        }
        else
        {
            characterObject.Mana += amount;
        }
        if (characterObject)
        {
            characterObject.OnManaChange?.Invoke(characterObject.Mana, characterObject.MaxMana);
        }
    }
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        stateMachine = GetComponent<StateMachine>();
    }
    public void ResetHealth()
    {

        characterObject.Health = characterObject.MaxHealth;
        characterObject.isDead = false;
    }

    public void AddToHealth(int amount)
    {
        if (characterObject.Health + amount < 0)
        {

            characterObject.Health = 0;

        }
        else if(characterObject.Health + amount >= characterObject.MaxHealth)
        {
            characterObject.Health = characterObject.MaxHealth;
        }
        else
        {

            characterObject.Health += amount;
        } 
        if (characterObject)
        {
            characterObject.OnHealthChange?.Invoke(characterObject.Health, characterObject.MaxHealth);
        }
    }

    public void TakeDamage(int amount, Entity attacker = null, HitBoxInfo hitbox = null)
    {
        lastAttacker = attacker;
        if (characterObject.Health - amount >= 0)
        {
            AddToHealth(-amount);

        }
        else
        {
            AddToHealth(-characterObject.Health);
        }
        if (characterObject.Health <= 0)
        {
            characterObject.isDead = true;
            stateMachine?.SetNextState(new DeadState());
            OnDead?.Invoke();
        }
        else
        {
            Vector2 impulse = lastAttacker != null?(transform.position - lastAttacker.transform.position).normalized:Vector2.right;
            if (characterObject.Armor <= 0)
            {
            damageChain += amount;

            if(damageChain >= ((characterObject.MaxHealth / 3)))
            {

                    stateMachine?.SetNextState(new KnockedDownState());
                    impulse.x *= Random.Range(8, 11);
                impulse = new Vector2(impulse.x, impulse.y);
                    damageChain = 0;
            }
            else
            {
                    if (hitbox != null)
                    {
                        impulse = hitbox.knockbackVector;
                        impulse *= hitbox.knockbackForce;
                    }
                    else
                    {
                        impulse.x *= Random.Range(1, 4);
                        impulse += 2 * Vector2.up;
                    }

                    if (hitbox == null)
                    {




                        stateMachine?.SetNextState(new HurtState());
                        
                    }
                    else
                    {
                        if (hitbox.stuns)
                        {
                            stateMachine?.SetNextState(new HurtState(hitbox.stunTime));
                        }
                        else
                        {

                            stateMachine?.SetNextState(new HurtState());
                        }
                    }


                }


                if (attacker)
                {
                    impulse.x *= attacker.GetComponent<BattleCharacter>().GetFacing();
                }
                rb.velocity = Vector3.zero;
                rb?.AddForce(impulse, ForceMode2D.Impulse);
            }
            else
            {
                characterObject.Armor -= amount;
            }
        }

        lastAttacker = null;

    }

   

    public bool CheckManaCost(AttackData attackData)
    {
        if(attackData == null)
        {
            return false;
        }
        return characterObject.Mana > attackData.manaCost;
    }

    private void OnDisable()
    {

    }




    public void ResetEntity()
    {
        ResetHealth();
    }


    private void Update()
    {
        if(damageChain > 0)
        {
            damageChain -= Time.deltaTime;
        }
        else
        {
            damageChain = 0;

        }
    }
}
