using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Command
{

    public BattleCharacter Source;
    public List<BattleCharacter> Target;
    public bool friendly = false;
    public delegate void CommandeEventHandler();
    public CommandeEventHandler OnExecuted;
    public Command()
    {

    }
    public virtual void ExecuteCommand()
    {

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

    public void SetTarget(List<BattleCharacter> _target)
    {
        Target = _target;
    }

    
}
