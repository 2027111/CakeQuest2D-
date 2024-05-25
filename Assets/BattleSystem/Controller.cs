using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class Controller : MonoBehaviour
{

    //public Vector2 wasdInput = Vector2.zero;
    public bool jump;
    public bool attack;

    public bool canControl = true;

    public bool canInteract = true;

    public delegate void EventHandler();

    public EventHandler OnJumpPressed;
    public EventHandler OnJumpRelease;
    public EventHandler OnReturnPressed;
    public EventHandler OnReturnReleased;
    public EventHandler OnSelectPressed;
    public EventHandler OnSelectReleased;



    public delegate void MovementHandler(Vector2 movement);
    public MovementHandler OnMovement;
    public MovementHandler OnMovementHeld;


    public void CanInteract(bool _canInteract)
    {
        canInteract = _canInteract;
    }
    public void CanMove(bool _canMove)
    {
        canControl = _canMove;
    }
    public bool AttackContains(Action interact)
    {
        if (OnSelectPressed == null)
        {
            return false;
        }
        return OnSelectPressed.GetInvocationList().Contains(interact);
    }

    public void Disable()
    {
        canControl = false;
    }

    public void Enable()
    {
        canControl = false;
    }
}
