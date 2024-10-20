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

    public static string controlSettings = "keyboard";
    private void Awake()
    {
        inputManager = this;
    }
    public void SetMove(InputAction.CallbackContext context)
    {
        Vector2 wasdInput = context.ReadValue<Vector2>().normalized;
        if (context.started)
        {
            OnMovementPressed?.Invoke(wasdInput);
        }
        OnMovementHeld?.Invoke(wasdInput);

    }
    public void OnInputChange(PlayerInput playerInput)
    {
        if (playerInput.currentControlScheme == "KeyboardControls")
        {
            controlSettings = "keyboard";
        }
        else
        {
            controlSettings = "controller";
        }
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

    public void OnSelect(InputAction.CallbackContext context)
    {
        if (canInteract)
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


        if (context.performed)
        {
            OnSecretSelectPressed?.Invoke();
        }
    }

    public void OnReturn(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            OnReturnPressed?.Invoke();
        }
        if (context.canceled)
        {
            OnReturnReleased?.Invoke();
        }
        attack = context.action.triggered;
    }
    public void OnPause(InputAction.CallbackContext callback)
    {
        // Check if the interaction is a press (button down)
        if (callback.started)
        {
            OnPausedPressed?.Invoke();
        }
    }

}
