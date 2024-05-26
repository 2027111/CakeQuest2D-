using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControlsBehaviour : CharacterBehaviour
{


    Controller playerController;
    
    public PlayerControlsBehaviour() : base()
    {
    }



    public override void OnEnter(Character player)
    {
        base.OnEnter(player);
        playerController = player.GetComponent<Controller>();
        if(playerController == null)
        {
            player.ChangeState(new PatrollingBehaviour());
        }
        else
        {
            character.inputManager.CanInteract(true);
            character.CanMove(true); 
            character.ActivateControls();
        }

    }



    public override void Handle()
    {
        if (movement.movementInput != Vector2.zero)
        {
            movement.MoveCharacter();
        }
    }


    public override void OnExit()
    {
        if (playerController != null)
        {
            character.inputManager?.CanInteract(false);
            character.CanMove(false);
        }
        movement.movementInput = Vector2.zero;
    }








}
