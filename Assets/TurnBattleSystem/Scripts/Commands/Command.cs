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
    public bool canCombo = true;
    public Command nextCommand;
    public delegate void CommandeEventHandler();
    public CommandeEventHandler OnExecuted;
    public CommandeEventHandler OnRecipeMatched;


    public string commandID;

    public Vector3 GetTargetPosition()
    {
        if (Target.Count == 0)
        {
            return Vector3.zero;
        }
        return Target[UnityEngine.Random.Range(0, Target.Count)].transform.position; ;
    }

    public Command()
    {
        this.commandID = Guid.NewGuid().ToString();
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

    public bool CanCombo()
    {
        return canCombo;
    }

    public virtual bool CanBeTarget(BattleCharacter _character)
    {
        return !_character.Entity.isDead;
    }


    public virtual void ActivateCommand()
    {

    }
    public virtual void ActivateCommand(BattleCharacter _target)
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


    public virtual bool WillKokusen()
    {
        foreach (BattleCharacter t in Target)
        {
            if (t.WillKokusen(this))
            {
                return true;
            }
        }
        return false;

    }

    public IEnumerator GoToOriginalPosition()
    {
        yield return GoToOriginalPosition(Source);
    }


    public bool IsInAttackPosition(BattleCharacter bc)
    {
        return bc.isInAttackPos;
    }
    public IEnumerator GoToOriginalPosition(BattleCharacter bc)
    {
        Vector3 currentPosition = bc.transform.position;
        float t = 0;
        bc.Animator.Move(true);
        if(Vector3.Distance(BattleManager.Singleton.GetPosition(bc), currentPosition) > .3f)
        {

        while (t < .5f)
        {

            bc.transform.position = Vector3.Lerp(currentPosition, BattleManager.Singleton.GetPosition(bc), t / .5f);
            t += Time.deltaTime;
            yield return null;
            }
        }
        bc.isInAttackPos = false;
        bc.Animator.Move(false);
    }
    public IEnumerator GoToEnemy(BattleCharacter bc)
    {
        Vector3 startPos = bc.transform.position;
        float t = 0;

        // Déclencher l'animation de mouvement
        bc.Animator.Move(true);

        // Calculer la différence de position entre l'EnemyContainer et le BattleCharacter
        Vector3 containerOffset = bc.EnemyContainer.transform.position - bc.transform.position;

        // Calculer la position cible pour le BattleCharacter en tenant compte de l'offset
        Vector3 targetPos = Target[0].transform.position - containerOffset;

        // Mouvement vers la position cible
        while (t < .5f)
        {
            bc.transform.position = Vector3.Lerp(startPos, targetPos, t / .5f);
            t += Time.deltaTime;
            yield return null;
        }

        // Position atteinte
        bc.isInAttackPos = true;
        bc.Animator.Move(false);
    }

    public void OnMatched()
    {

    }

    public virtual Element GetElement()
    {
        return Source.GetReference().AttackElement;
    }

    public IEnumerator GoToEnemy()
    {
        yield return GoToEnemy(Source);
    }

    public void SetNewId()
    {
        this.commandID = Guid.NewGuid().ToString();
    }
}
