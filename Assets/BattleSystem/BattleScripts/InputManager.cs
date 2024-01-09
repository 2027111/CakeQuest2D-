using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;


[RequireComponent(typeof(PlayerInput))]


public class InputManager : Controller
{
    

    private void Start()
    {

        SetController();
    }
    public void SetMove(InputAction.CallbackContext context)
    {
        wasdInput = context.ReadValue<Vector2>();
        OnMovement?.Invoke(wasdInput);
    }

    private void OnDestroy()
    {

        Destroy(GetComponent<PlayerInput>());
    }


    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            OnJumpPressed?.Invoke();
        }
        if (context.canceled)
        {
            OnJumpRelease?.Invoke();
        }
        jump = context.action.triggered;
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            OnAttackPressed?.Invoke();
        }
        if (context.canceled)
        {
            OnAttackRelease?.Invoke();
        }
        attack = context.action.triggered;
    }

}
