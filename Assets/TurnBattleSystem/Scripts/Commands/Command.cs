using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Command
{

    public Vector3 startPosition;
    public BattleCharacter Source;
    public List<BattleCharacter> Target;
    public Friendliness friendliness = Friendliness.Non_Friendly;
    public TargetStateType targetStateType = TargetStateType.Alive;
    public bool skippable = true;
    public bool canFocus = true;
    public delegate void CommandeEventHandler();
    public CommandeEventHandler OnExecuted;
    public Command()
    {

    }
    public virtual void ExecuteCommand()
    {
        Source.isActing = true;
    }


    public virtual void OnCommandOver()
    {
        Source.isActing = false;
    }

    public bool CanFocus()
    {
        return canFocus;
    }

    public virtual bool CanBeTarget(BattleCharacter _character)
    {
        return !_character.Entity.isDead;
    }


    public virtual void ActivateCommand()
    {

    }
    public void SetSource(BattleCharacter _source)
    {
            Source = _source;
    }

    public virtual void SetTarget(List<BattleCharacter> _target)
    {
        Target = _target;
    }

    public IEnumerator GoToOriginalPosition()
    {
        Vector3 currentPosition = Source.transform.position;
        float t = 0;
        Source.Animator.Move(true);
        while (t < .5f)
        {

            Source.transform.position = Vector3.Lerp(currentPosition, BattleManager.Singleton.GetPosition(Source), t / .5f);
            t += Time.deltaTime;
            yield return null;
        }
        Source.Animator.Move(false);

    }
    public IEnumerator GoToOriginalPosition(BattleCharacter bc)
    {
        Vector3 currentPosition = bc.transform.position;
        float t = 0;
        bc.Animator.Move(true);
        while (t < .5f)
        {

            bc.transform.position = Vector3.Lerp(currentPosition, BattleManager.Singleton.GetPosition(bc), t / .5f);
            t += Time.deltaTime;
            yield return null;
        }
        bc.Animator.Move(false);
    }

    public IEnumerator GoToEnemy()
    {
        Vector3 pos = Source.transform.position;
        float t = 0;

        Source.Animator.Move(true);

        Vector3 diff = (Target[0].transform.position - Source.transform.position).normalized;
        while (t < .5f)
        {
            Source.transform.position = Vector3.Lerp(pos, Target[0].transform.position - diff * 1.5f, t / .5f);
            t += Time.deltaTime;
            yield return null;
        }
        Source.Animator.Move(false);
    }
}
