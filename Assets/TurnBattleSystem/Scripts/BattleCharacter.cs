
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
    public CharacterState currentState;
    public BattleCharacter _target;


    public bool isActing = false;

    public bool isBlocking = false;
    public bool isParrying = false;




    public Command currentCommand;


    public AnimatorController Animator;

    public Entity Entity;



 

    void Start()
    {
        //speed = currentCharacter.Speed;
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

    public void Block()
    {
        isBlocking = true;
        Animator.Block();
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

    public bool CanAct()
    {
        return !isActing;
    }

    public Command CreateCommand()
    {
        float prob = Random.Range(0f, 100f);
        if(prob > 50)
        {
            return new AttackCommand();
        }
        else
        {
            Attack attack = GetRandomAttack();
            if (attack)
            {
                return new SkillCommand(attack);
            }
            else
            {
                return new AttackCommand();
            }
        }
    }

    private Attack GetRandomAttack()
    {

        List<Attack> returnAttacks = new List<Attack>();
        List<Attack> possibleAttacks = GetAttacks();
        foreach(Attack attack in possibleAttacks){
            if (BattleManager.Singleton.GetPossibleTarget(attack, this).Count > 0)
            {
                returnAttacks.Add(attack);
            }
        }
        if(returnAttacks.Count == 0)
        {
            return null;
        }
        return returnAttacks[Random.Range(0, returnAttacks.Count)];
    }

    public List<Attack> GetAttacks()
    {
        return GetReference().Attacks;
    }

    public void StartParryWindow()
    {
        StartCoroutine(Parry(GetReference().GetParryWindowTime()));
    }
    IEnumerator Parry(float duration)
    {
        Parry();
        float t = 0;
        while (t < duration)
        {
            t += Time.deltaTime;
            yield return null;
        }


        StopParry();
    }

    public void StopParry()
    {
        isParrying = false;
        GetComponentInChildren<SpriteRenderer>().color = Color.white;
    }

    public void Parry()
    {

        isParrying = true;
        GetComponentInChildren<SpriteRenderer>().color = Color.red;
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

    public bool IsTargetted()
    {
        List<BattleCharacter> targets = BattleManager.Singleton.GetCurrentTarget();
        return targets.Count > 0? targets.Contains(this):false;
    }

    public void SetTeam(TeamIndex index)
    {
        GetComponent<TeamComponent>().teamIndex = index;
    }

    public void StopBlock()
    {
        Animator.StopBlock();
        isBlocking = false;
    }
}
