using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackCommand : Command
{
    public Vector3 startPosition;
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

    public IEnumerator GoToOriginalPosition()
    {
        Vector3 currentPosition = Source.transform.position;
        float t = 0;
        Source.Animator.Move(true);
        while (t < .5f)
        {

            Source.transform.position = Vector3.Lerp(currentPosition, startPosition, t / .5f);
            t += Time.deltaTime;
            yield return null;
        }
        Source.Animator.Move(false);

    }

    public IEnumerator GoToEnemy()
    {
        float t = 0;

        Source.Animator.Move(true);

        Vector3 diff = (Target[0].transform.position - Source.transform.position).normalized;
        while (t < .5f)
        {
            Source.transform.position = Vector3.Lerp(startPosition, Target[0].transform.position - diff * 1.5f, t / .5f);
            t += Time.deltaTime;
            yield return null;
        }
        Source.Animator.Move(false);
    }

    public override void ActivateCommand()
    {
        foreach(BattleCharacter target in Target)
        {
            CharacterObject characterObject = target.GetReference();
            ElementEffect elementEffect = characterObject.GetElementEffect(Source.GetReference().AttackElement);
            target?.Entity.TakeDamage(-Source.GetReference().AttackDamage, elementEffect);
            CamManager.Shake(.2f, .05f);
            base.ActivateCommand();
        }
    }

}
