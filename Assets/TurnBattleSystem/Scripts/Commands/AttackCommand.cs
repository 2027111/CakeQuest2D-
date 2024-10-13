using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackCommand : Command
{
    public AttackInformation attackInformation;
    public override void ExecuteCommand()
    {
        Source.StartCoroutine(Execute());
    }


    public virtual IEnumerator Execute()
    {

        startPosition = Source.transform.position;

        if (IsPhysical() && !IsInAttackPosition())
        {
            
            yield return Source.StartCoroutine(GoToEnemy());
            yield return new WaitForSeconds(.6f);
        }
        yield return new WaitForSeconds(.6f);
        if (WillKokusen())
        {
            if (IsPhysical())
            {
                CamManager.PanToCharacter(Source);
            }
            GameObject.Instantiate(BattleManager.Singleton?.KOKUSENAURA, Source.transform.position + Vector3.up, Quaternion.identity);
            BattleManager.Singleton.FadeBackground(true);
            Source.StartCoroutine(Utils.SlowDown(1f, .02f));
        }
        yield return Source.StartCoroutine(WaitForAnimationOver());

        yield return new WaitForSeconds(.6f);
        CamManager.ResetView();
        BattleManager.Singleton.FadeBackground(false, .3f);
        if (IsPhysical() || IsInAttackPosition())
        {
            yield return Source.StartCoroutine(GoToOriginalPosition());
            yield return new WaitForSeconds(.6f);
        }
        OnCommandOver();
        OnExecuted?.Invoke();


    }

    private bool IsInAttackPosition()
    {
        return IsInAttackPosition(Source);
    }

    public virtual bool IsPhysical()
    {
        return Source.GetReference().attackType == SkillType.Physical;
    }
    public override void SetTarget(List<BattleCharacter> _target)
    {
        Target = new List<BattleCharacter>();
        Target.Add(_target[Random.Range(0, _target.Count)]);
    }

    public override void ActivateCommand()
    {
        foreach (BattleCharacter target in Target)
        {
            ActivateCommand(target);
        }
        base.ActivateCommand();
    }

    public virtual IEnumerator WaitForAnimationOver()
    {
        AnimationClip clip = null;
        Source.ResetAnimatorController();
        Source.Animator.Attack();
        yield return new WaitForSeconds(.1f);
        clip = Source.Animator.GetCurrentAnim().clip;
        while (Source.Animator.GetCurrentAnim().clip == clip && Source.Animator.GetCurrentAnimTime() < Source.Animator.GetCurrentAnimLen())
        {
            yield return null;
        }

        Source.ResetAnimatorController();
    }

    public override void ActivateCommand(BattleCharacter _target)
        {
            CharacterObject characterObject = _target.GetReference();
            ElementEffect elementEffect = characterObject.GetElementEffect(Source.GetReference().AttackElement);
        if (attackInformation == null)
        {
            attackInformation = new AttackInformation(null, elementEffect, Source, BattleManager.Singleton.GetActor().currentCommand);
        }
        attackInformation.element = Source.GetReference().AttackElement;
            _target?.Entity.AddToHealth(attackInformation);
            CamManager.Shake(.2f, .05f);
            base.ActivateCommand();
        
        }

}
