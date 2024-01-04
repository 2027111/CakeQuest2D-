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
        character.canInteract = true;
    }



    public override void Handle()
    {
    }


    public override void OnExit()
    {
        character.canInteract = false;

    }






}
