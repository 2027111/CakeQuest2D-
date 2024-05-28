using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemCommand : Command
{

    protected BattleItem item;

    public ItemCommand(BattleItem _item)
    {
        item = _item;
        friendliness = item.friendliness;
        canFocus = false;
    }
    public override void ExecuteCommand()
    {
        Debug.Log("Used " + item.itemName);
        base.ExecuteCommand();
        Source.StartCoroutine(Execute());
    }

    public IEnumerator Execute()
    {

        startPosition = Source.transform.position;
        yield return new WaitForSeconds(.6f);

        yield return Source.StartCoroutine(WaitForAnimationOver());

        yield return new WaitForSeconds(.6f);
        OnCommandOver();
        OnExecuted?.Invoke();


    }


    public override void ActivateCommand()
    {
        item.BattleUse(Target, BattleManager.Singleton.playerInventory);
    }
    public override bool CanBeTarget(BattleCharacter _character)
    {

        return base.CanBeTarget(_character) == (item.targetStateType == TargetStateType.Alive);
    }

    public override void SetTarget(List<BattleCharacter> _target)
    {
        Target = new List<BattleCharacter>();
        if (item.targetType == TargetType.Single)
        {
            Target.Add(_target[Random.Range(0, _target.Count)]);
        }
        else
        {
            Target = _target;
        }
    }
    public IEnumerator WaitForAnimationOver()
    {
        Source.Animator.Attack();
        yield return new WaitForSeconds(.1f);
        AnimationClip clip = Source.Animator.GetCurrentAnim().clip;
        while (Source.Animator.GetCurrentAnim().clip == clip && Source.Animator.GetCurrentAnimTime() < Source.Animator.GetCurrentAnimLen())
        {
            yield return null;
        }
        Source.ResetAnimatorController();

    }
    public BattleItem GetItem()
    {
        return item;
    }
}
