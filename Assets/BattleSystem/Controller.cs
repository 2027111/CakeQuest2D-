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
    public EventHandler OnSpecialPressed;
    public EventHandler OnSpecialRelease;
    public EventHandler OnAttackPressed;
    public EventHandler OnAttackRelease;
    public delegate void MovementHandler(Vector2 movement);
    public MovementHandler OnMovement;

    public void SetController()
    {
        GetComponent<Player>().SetController(this);
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
