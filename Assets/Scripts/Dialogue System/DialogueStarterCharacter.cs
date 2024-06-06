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
            StopCharacterBehaviour();
            base.DialogueAction();
        }
    }


    public void StopCharacterBehaviour()
    {
        character = GetComponent<Character>();
        defaultBehaviourType = character.GetCurrentBehaviour().GetType();
        character.ChangeState(new NothingBehaviour());
        character.LookAt(Character.Player.gameObject);
    }

    public override void DialogueOver()
    {
        // Change the state back to the original default behavior
        ResumeCharacterBehaviour();
        base.DialogueOver();
    }



    public void ResumeCharacterBehaviour()
    {

        character.ChangeState(Activator.CreateInstance(defaultBehaviourType) as CharacterBehaviour);
    }
}
