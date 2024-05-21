using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillCommand : AttackCommand
{

    protected Attack attack;
    public SkillCommand(Attack _attack)
    {
        attack = _attack;
        friendly = attack.GetFriendly();
    }
    public override void ExecuteCommand()
    {
        Source.Entity.AddToMana(-attack.manaCost);
        base.ExecuteCommand();
    }

    public override IEnumerator Execute()
    {

        startPosition = Source.transform.position;

        if (attack.skillType == SkillType.Physical)
        {
            yield return Source.StartCoroutine(GoToEnemy());
            yield return new WaitForSeconds(.6f);
        }
        yield return new WaitForSeconds(.6f);

        yield return Source.StartCoroutine(WaitForAnimationOver());

        yield return new WaitForSeconds(.6f);
        if(attack.skillType == SkillType.Physical)
        {
            yield return Source.StartCoroutine(GoToOriginalPosition());
            yield return new WaitForSeconds(.6f);
            Source.ResetAnimatorController();
        }

        OnExecuted?.Invoke();


    }


    public override void ActivateCommand()
    {


        foreach (BattleCharacter target in Target)
        {
            if(target.Entity.isDead == false)
            {

            float sourceSpeed = Source.Speed * (attack.baseAccuracy / 100f);
            float targetSpeed = target.Speed;
            float adjustedSpeed = sourceSpeed - targetSpeed * 0.5f; // Weigh target speed less to favor source speed

            float dodgeThreshold = adjustedSpeed + targetSpeed;
            float dodge = Random.Range(0f, dodgeThreshold);

            if (dodge > sourceSpeed)
            {
                target.Animator.Dodge();
                target.GetComponent<TextEffect>().SpawnTextEffect("Miss", Color.white);
            }
            else
            {
                    if (attack.HitEffect)
                    {
                        GameObject.Instantiate(attack.HitEffect, target.transform.position + Vector3.up, Quaternion.identity);
                    }
                target?.Entity.TakeDamage(attack.baseDamage, Source);
            }
            }

        }

    }
    public override bool CanBeTarget(BattleCharacter _character)
    {

        return base.CanBeTarget(_character) == (attack.targetStateType == TargetStateType.Alive);
    }
    public IEnumerator WaitForAnimationOver()
    {
        Source.Animator.Attack();
        Source.ApplyAttackAnimationOverride(attack);
        yield return new WaitForSeconds(.1f);
        while (Source.Animator.GetCurrentAnim().clip == attack.animationClip && Source.Animator.GetCurrentAnimTime() < Source.Animator.GetCurrentAnimLen())
        {
            yield return null;
        }
        Source.ResetAnimatorController();

    }
    public Attack GetAttack()
    {
        return attack;
    }
}
