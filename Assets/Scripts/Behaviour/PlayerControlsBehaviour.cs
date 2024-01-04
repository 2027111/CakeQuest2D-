using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControlsBehaviour : CharacterBehaviour
{


    PlayerInputController playerController;
    
    public PlayerControlsBehaviour() : base()
    {
    }



    public override void OnEnter(Character player)
    {
        base.OnEnter(player);
        playerController = player.GetComponent<PlayerInputController>();
        if(playerController == null)
        {
            player.ChangeState(new PatrollingBehaviour());
        }
        player.canInteract = true;

    }



    public override void Handle()
    {
        movement.movementInput = playerController.input;
        if (movement.movementInput != Vector2.zero)
        {
            movement.MoveCharacter();
        }
    }


    public override void OnExit()
    {
        movement.movementInput = Vector2.zero;
        character.canInteract = false;
    }








}
