using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractingBehaviour : CharacterBehaviour
{
    public InteractingBehaviour() : base()
    {
    }



    public override void OnEnter(Character player)
    {
        base.OnEnter(player);
        character.inputManager.CanInteract(true);
    }



    public override void Handle()
    {
    }


    public override void OnExit()
    {
        character.inputManager.CanInteract(false);

    }






}
