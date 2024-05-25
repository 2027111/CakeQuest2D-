using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackCommand : Command
{
    public override void ExecuteCommand()
    {
        Source.StartCoroutine(Execute());
    }

    public virtual IEnumerator Execute()
    {
        startPosition = Source.transform.position;
        yield return Source.StartCoroutine(GoToEnemy());


        yield return new WaitForSeconds(.3f);

        Source.Animator.Move(false);
        Source.Animator.Attack();


        float animTime = 0;
        while (animTime < Source.Animator.GetCurrentAnimLen())
        {
            animTime += Time.deltaTime;
            yield return null;
        }


        yield return new WaitForSeconds(.6f);

        yield return Source.StartCoroutine(GoToOriginalPosition());

        yield return new WaitForSeconds(.6f);
        OnExecuted?.Invoke();

    }



    public override void ActivateCommand()
    {
        foreach(BattleCharacter target in Target)
        {
            CharacterObject characterObject = target.GetReference();
            ElementEffect elementEffect = characterObject.GetElementEffect(Source.GetReference().AttackElement);
            target?.Entity.TakeDamage(-Source.GetReference().AttackDamage, elementEffect, Source);
            CamManager.Shake(.2f, .05f);
            base.ActivateCommand();
        }
    }

}
