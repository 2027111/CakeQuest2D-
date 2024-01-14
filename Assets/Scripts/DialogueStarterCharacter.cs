using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueStarterCharacter : NewDialogueStarterObject
{

    Type defaultBehaviourType;
    Character character;



   
    public override void DialogueAction()
    {

        if (!started)
        {
            character = GetComponent<Character>();
            defaultBehaviourType = character.GetCurrentBehaviour().GetType();
            Debug.Log(defaultBehaviourType);
            character.ChangeState(new NothingBehaviour());
            Debug.Log(character.GetCurrentBehaviour().GetType());
            character.LookAt(player.gameObject);
            base.DialogueAction();
        }
    }

    public override void DialogueOver()
    {
        // Change the state back to the original default behavior
        character.ChangeState(Activator.CreateInstance(defaultBehaviourType) as CharacterBehaviour);
        Debug.Log("Test");
        base.DialogueOver();
    }
}
