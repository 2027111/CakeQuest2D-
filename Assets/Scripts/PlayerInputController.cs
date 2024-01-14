using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputController : MonoBehaviour
{
    public Vector2 input;

    Character pm;



    private void Start()
    {
        pm = GetComponent<Character>();
    }

    public void OnMove(InputAction.CallbackContext callback)
    {
        input = callback.ReadValue<Vector2>();
    }

    public void OnInteract(InputAction.CallbackContext callback)
    {
        // Check if the interaction is a press (button down)
        if (callback.started)
        {
            // Implement your interaction logic here
            // For example, you might call a method to interact with an object.
            Interact();
        }
    }

    public void OnPause(InputAction.CallbackContext callback)
    {
        // Check if the interaction is a press (button down)
        if (callback.started)
        {
            Debug.Log("Pause");
            Pause();

        }
    }

    public void OnRun(InputAction.CallbackContext callback)
    {
        // Check if the interaction is a press (button down)
        if (callback.started)
        {
            pm.Run(true);

        }
        else if (callback.canceled)
        {
            pm.Run(false);
        }
    }

    private void Pause()
    {
        PauseManager.Singleton?.OnPausePressed();
    }

    void Interact()
    {
        // Implement your interaction logic here
        // For example, interact with an object in the game world.
        pm.InteractPressed();
    }
}
