using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillCommand : AttackCommand
{

    protected Skill attack;

    public SkillCommand(Skill _attack)
    {
        attack = _attack;
        friendliness = attack.friendliness;
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
        OnCommandOver();
        OnExecuted?.Invoke();


    }
    public override void SetTarget(List<BattleCharacter> _target)
    {
        Target = new List<BattleCharacter>();
        if(attack.targetType == TargetType.Single)
        {
            Target.Add(_target[Random.Range(0, _target.Count)]);
        }
        else
        {
            Target = _target;
        }
    }

    public override void ActivateCommand()
    {
        attack.UseSkill(Source, Target);
    }

    public override void ActivateCommand(BattleCharacter _target)
    {
        List<BattleCharacter> temp = new List<BattleCharacter>();
        temp.Add(_target);
        attack.UseSkill(Source, temp);

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
    public Skill GetAttack()
    {
        return attack;
    }
}
