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

        // Round wasdInput to the closest cardinal or diagonal direction
        Vector2 roundedInput = RoundToCardinalDiagonal(wasdInput);
        Vector2 cardinalInput = RoundToCardinal(wasdInput);

        // Only update if movement is different from the previous movement
        if (roundedInput == Vector2.zero)
        {
            if (movement != Vector2.zero)
            {
                OnMovementStopped?.Invoke(roundedInput);
            }
        }
        else
        {
            if (movement == Vector2.zero || RoundToCardinal(movement) != cardinalInput)
            {
                OnMovementPressed?.Invoke(cardinalInput);
            }
        }

        if (movement != roundedInput)
        {
            movement = roundedInput;
            OnMovementHeld?.Invoke(roundedInput);
        }
    }

    private Vector2 RoundToCardinalDiagonal(Vector2 input)
    {
        if (input == Vector2.zero)
            return Vector2.zero;

        float angle = Mathf.Atan2(input.y, input.x) * Mathf.Rad2Deg;
        angle = Mathf.Round(angle / 45f) * 45f; // Round to the nearest 45 degrees

        float radians = angle * Mathf.Deg2Rad;
        return new Vector2(Mathf.Cos(radians), Mathf.Sin(radians)).normalized;
    }

    private Vector2 RoundToCardinal(Vector2 input)
    {
        if (input == Vector2.zero)
            return Vector2.zero;

        float angle = Mathf.Atan2(input.y, input.x) * Mathf.Rad2Deg;
        angle = Mathf.Round(angle / 90f) * 90f; // Round to the nearest 90 degrees (cardinal directions only)

        float radians = angle * Mathf.Deg2Rad;
        return new Vector2(Mathf.Cos(radians), Mathf.Sin(radians)).normalized;
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
            JumpIsPressed = true;
            OnJumpPressed?.Invoke();
        }
        if (context.canceled)
        {
            JumpIsPressed = false ;
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
                SelectIsPressed = true;
                OnSelectPressed?.Invoke();
            }
            if (context.canceled)
            {
                SelectIsPressed = false;
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
            ReturnIsPressed = true;
            OnReturnPressed?.Invoke();
        }
        if (context.canceled)
        {
            ReturnIsPressed = false;
            OnReturnReleased?.Invoke();
        }
        attack = context.action.triggered;
    }
    public void OnPause(InputAction.CallbackContext callback)
    {
        // Check if the interaction is a press (button down)
        if (callback.started)
        {
            PauseIsPressed = true;
            OnPausedPressed?.Invoke();
        }
        if (callback.canceled)
        {
            PauseIsPressed = false;
            OnPausedReleased?.Invoke();
        }
    }

}
