using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleObjects : MonoBehaviour
{


    protected Command command;
    protected BattleCharacter target;
    public delegate void CommandeEventHandler();
    public CommandeEventHandler OnOver;



    public virtual void SetCommand(Command _command)
    {
        command = _command;
    }
    public virtual void SetTarget(BattleCharacter _target)
    {
        target = _target;
    }

    private void OnDestroy()
    {
        OnOver?.Invoke();
    }

    public void TriggerHit()
    {
        command?.ActivateCommand(target);
    }

}
