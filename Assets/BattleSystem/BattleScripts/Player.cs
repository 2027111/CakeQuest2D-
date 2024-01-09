using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public delegate void EventHandler();
    public EventHandler OnPlayerPrefabLocalSpawn;
    public Controller inputManager;
    public BattleCharacter character;

    // Start is called before the first frame update
    void Start()
    {

        // Set up the controller
        SetController(inputManager);
    }

    public void SetController(Controller aIController)
    {
        character = GetComponent<BattleCharacter>();
        inputManager = aIController;
        SubscribeToInputEvents();
    }

    private void OnDestroy()
    {
        if (inputManager)
        {
            UnsubscribeFromInputEvents();
            Destroy(inputManager);
        }
    }

    private void SubscribeToInputEvents()
    {
        if (inputManager)
        {
            inputManager.OnMovement += character.SetMove;
            inputManager.OnAttackPressed += character.OnAttackPress;
            inputManager.OnAttackRelease += character.OnAttackLetGo;
            inputManager.OnJumpPressed += character.OnJumpPress;
            inputManager.OnJumpRelease += character.OnJumpLetGo;
        }
    }

    private void UnsubscribeFromInputEvents()
    {
        if (inputManager)
        {
            inputManager.OnMovement -= character.SetMove;
            inputManager.OnAttackPressed -= character.OnAttackPress;
            inputManager.OnAttackRelease -= character.OnAttackLetGo;
            inputManager.OnJumpPressed -= character.OnJumpPress;
            inputManager.OnJumpRelease -= character.OnJumpLetGo;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetDirectionInput(Vector2 movementinput)
    {
        character.direction = movementinput;
    }
}
