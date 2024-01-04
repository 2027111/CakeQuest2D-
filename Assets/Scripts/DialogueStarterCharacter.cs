using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueStarterCharacter : DialogueStarterObject
{

    Type defaultBehaviourType;
    Character character;



   
    public override void DialogueAction()
    {
        character = GetComponent<Character>();
        defaultBehaviourType = character.GetCurrentBehaviour().GetType();
        character.ChangeState(new NothingBehaviour());
        character.LookAt(player.gameObject);
        base.DialogueAction();
    }

    public override void DialogueOver()
    {

        // Change the state back to the original default behavior
        character.ChangeState(Activator.CreateInstance(defaultBehaviourType) as CharacterBehaviour);
    }
}
