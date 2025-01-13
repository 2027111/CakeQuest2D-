using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControlsBehaviour : CharacterBehaviour
{




    public PlayerControlsBehaviour() : base()
    {
    }



    public override void OnEnter(Character player)
    {
        base.OnEnter(player);
        if (character.inputManager == null || Timeline.IsInCutscene || (FadeScreen.movingScene))
        {
            player.ChangeState(new PatrollingBehaviour());
            return;
        }
        else
        {
            character.inputManager.CanInteract(true);
            character.CanMove(true);
            character.ActivateControls();
            NothingBehaviour.skipTimer = 0;
            UICanvas.SetSkipPanel(0);
        }
        if (player.gameObject.CompareTag("Player"))
        {
            UICanvas.TurnBordersOn(true);
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
        if (character.inputManager != null)
        {
            character.inputManager?.CanInteract(false);
            character.CanMove(false);
            character.ActivateControls(false);
        }
        movement.movementInput = Vector2.zero;

    }








}
