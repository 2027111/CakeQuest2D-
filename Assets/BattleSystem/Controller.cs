using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Controller : MonoBehaviour
{

    public Vector2 wasdInput = Vector2.zero;
    public bool jump;
    public bool attack;

    public bool canControl = true;

    public delegate void EventHandler();
    public EventHandler OnJumpPressed;
    public EventHandler OnJumpRelease;
    public EventHandler OnReturnPressed;
    public EventHandler OnReturnRelease;
    public EventHandler OnSelectPressed;
    public EventHandler OnSelectReleased;
    public delegate void MovementHandler(Vector2 movement);
    public MovementHandler OnMovement;



    public void Disable()
    {
        canControl = false;
    }

    public void Enable()
    {
        canControl = false;
    }
}
