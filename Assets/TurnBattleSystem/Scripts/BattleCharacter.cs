using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CharacterState
{
    Idle,
    Choosing,
    Died
}
[RequireComponent(typeof(TeamComponent))]
public class BattleCharacter : MonoBehaviour
{

    [SerializeField] CharacterObject currentCharacter;
    public int Speed = 50;
    public int Health = 50;
    public int Mana = 50;
    public CharacterState currentState;
    public BattleCharacter _target;


    public bool isActing = false;


    public delegate void EventHandler(int health, int maxhealth);
    public EventHandler OnHealthChange;
    public EventHandler OnManaChange;



    public Command currentCommand;
    public AnimatorController Animator;

    public Entity Entity;





    void Start()
    {
        //speed = currentCharacter.Speed;
        Health = currentCharacter.Health;
        Mana = currentCharacter.Mana;
        Animator = GetComponent<AnimatorController>();
        Entity = GetComponent<Entity>();
        GetComponentInChildren<SpriteEvents>().SetCharacter(this);
    }

    
    private void Update()
    {
        switch (currentState)
        {
            case CharacterState.Idle:
                break;
            case CharacterState.Choosing:
                
                break;
            case CharacterState.Died:
                break;
        }
    }

    public void SetActing(bool _isActing)
    {
        isActing = _isActing;
    }
    public CharacterData GetData()
    {
        return currentCharacter.characterData;
    }

    public CharacterObject GetReference()
    {
        return currentCharacter;
    }

    public void Hurt(int damage)
    {
        Health -= damage;
    }

    public Command CreateCommand()
    {
        return new AttackCommand();
    }


    public List<Attack> GetAttacks()
    {
        return GetReference().Attacks;
    }



    public bool IsPlayerTeam()
    {
        return GetComponent<TeamComponent>().teamIndex == TeamIndex.Player;
    }

    public TeamIndex GetTeam()
    {
        return GetComponent<TeamComponent>().teamIndex;
    }
    public Attack GetAttack(string v)
    {
        foreach(Attack attack in GetAttacks())
        {
            if(attack.ToString() == v)
            {
                return attack;
            }
        }
        return null;
    }

    public void TriggerHit()
    {
        currentCommand.ActivateCommand();
    }


    public void ApplyAttackAnimationOverride(Attack attack)
    {
        AnimatorOverrideController originalController = new AnimatorOverrideController(Animator.GetController());
        var anims = new List<KeyValuePair<AnimationClip, AnimationClip>>();
        foreach (var a in originalController.animationClips)
        {
            if (a.name == "Attack_temp")
            {
                anims.Add(new KeyValuePair<AnimationClip, AnimationClip>(a, attack.animationClip));
            }
        }
        originalController.ApplyOverrides(anims);
        Animator.SetController(originalController);
    }



    public void ResetAnimatorController()
    {
        Animator.SetController(GetReference().animationController);
    }

    public void SetReference(CharacterObject characterObject)
    {
        currentCharacter = characterObject;
        ResetAnimatorController();
    }

    public void Flip(int flipIndex)
    {
        transform.localScale = new Vector3(flipIndex, 1, 1);
    }
}
