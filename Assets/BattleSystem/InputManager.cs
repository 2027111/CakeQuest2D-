using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;
using System;
using System.Linq;

[RequireComponent(typeof(PlayerInput))]


public class InputManager : Controller
{
    public static InputManager inputManager;

    private void Awake()
    {
        inputManager = this;
    }
    public void SetMove(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            wasdInput = context.ReadValue<Vector2>();
            OnMovement?.Invoke(wasdInput);
        }
    }

    private void OnDestroy()
    {

        Destroy(GetComponent<PlayerInput>());
    }

    public bool AttackContains(Action interact)
    {
        if (OnSelectPressed == null)
        {
            return false;
        }
        return OnSelectPressed.GetInvocationList().Contains(interact);
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

    public void OnSelect(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            OnSelectPressed?.Invoke();
        }
        if (context.canceled)
        {
            OnSelectReleased?.Invoke();
        }
        attack = context.action.triggered;
    }

    public void OnReturn(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            OnReturnPressed?.Invoke();
        }
        if (context.canceled)
        {
            OnReturnRelease?.Invoke();
        }
        attack = context.action.triggered;
    }


}
