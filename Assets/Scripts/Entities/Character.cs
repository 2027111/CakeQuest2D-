using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Character : MonoBehaviour
{

    public static Character Player;
    CharacterBehaviour playerBehaviour;
    Movement playerMovement;
    public CharacterInventory inventory;
    public bool canGetInteract = true;
    public Vector2 input = Vector2.zero;
    //public event Action OnInteractEvent;
    public Controller inputManager;
    public Party heroParty;
    private CharacterBehaviour previousBehaviour;



    // Start is called before the first frame update
    private void Awake()
    {
        playerMovement = GetComponent<Movement>();
        inputManager = GetComponent<Controller>();



        ToggleNothingState();
        if (inputManager)
        {
            Player = this;
        }
        FadeScreen.AddOnEndFadeEvent(TogglePlayableState);


    }


    public void ActivateControls(bool on = true)
    {
        if (on)
        {

            inputManager.OnReturnPressed += delegate { Run(true); };
            inputManager.OnReturnReleased += delegate { Run(false); };
            inputManager.OnPausedPressed += delegate { PauseMenu.Singleton?.OnPausePressed(); };

        }
        else
        {

            inputManager.OnReturnPressed = null;
            inputManager.OnReturnReleased = null;
            inputManager.OnPausedPressed = null;
        }
    }

    public void Move(Vector2 input)
    {
        playerMovement.SetInput(input);
    }

    public void CanMove(bool v)
    {
        if (v)
        {
            inputManager.OnMovementHeld += Move;
        }
        else
        {
            inputManager.OnMovementHeld -= Move;
        }
    }

    public static void ActivatePlayer()
    {
        Player.TogglePlayableState();
    }

    public static void DeactivatePlayer()
    {
        Player.ToggleNothingState();
    }

    public bool CanInteraction()
    {
        return canGetInteract;
    }

    public void SetInteraction(bool interaction)
    {
        canGetInteract = interaction;
    }
 
    public void AddToInventory(InventoryItem content, int amount = 1)
    {
        if (inventory)
        {
            inventory.AddToInventory(content, amount);
        }
    }

    public bool HasObject(InventoryItem content, int amount = 1)
    {

        return inventory.HasObject(content, amount);
    }
    public int AmountObject(InventoryItem content)
    {
        return inventory.AmountObject(content);
        
    }
    public bool RemoveFromInventory(InventoryItem content, int amount = 1)
    {


        return inventory.RemoveFromInventory(content, amount);
    }

    public void Run(bool running)
    {
        playerMovement.Run(running);
    }

    public void LookToward(Vector2 direction)
    {
        playerMovement.LookAt(direction);
    }

    public void LookAt(GameObject target)
    {

        LookToward((target.transform.position - transform.position).normalized);
    }

    public void LookUp()
    {
        

        // Now you can use the lookDirection vector to orient the object
        LookToward(Vector2.up.normalized);
    }

    public void LookDown()
    {


        // Now you can use the lookDirection vector to orient the object
        LookToward(Vector2.down.normalized);
    }
    public void LookRight()
    {


        // Now you can use the lookDirection vector to orient the object
        LookToward(Vector2.right.normalized);
    }
    public void LookLeft()
    {


        // Now you can use the lookDirection vector to orient the object
        LookToward(Vector2.left.normalized);
    }

    public void SetPosition(Vector3 newPosition)
    {
        if (!playerMovement)
        {
            playerMovement = GetComponent<Movement>();
        }


        playerMovement?.SetPosition(newPosition);
    }

    public string GetState()
    {
        return GetCurrentBehaviour().GetType().ToString();
    }

    public void ChangeState(CharacterBehaviour newBehaviour)
    {

        if (FadeScreen.fading)
        {
            return;
        }

        playerBehaviour?.OnExit();
        previousBehaviour = playerBehaviour;
        playerBehaviour = newBehaviour;
        if (previousBehaviour != null)
        {
            //Debug.Log(name + " State changed : " + previousBehaviour.GetType().ToString() + " replaced by " + GetCurrentBehaviour().ToString(),this);
        }
        newBehaviour.OnEnter(this);
    }

    public CharacterBehaviour GetCurrentBehaviour()
    {
        return playerBehaviour;
    }

    private void FixedUpdate()
    {
        playerBehaviour.Handle();
    }

    public void TogglePlayableState()
    {
        if (Player == this && !Timeline.IsInCutscene)
        {
            ChangeState(new PlayerControlsBehaviour());

        }
        else
        {

            ChangeState(new PatrollingBehaviour());

        }
    }

    public void ToggleCutsceneState()
    {
        ChangeState(new NothingBehaviour());
    }

    public void TogglePreviousState()
    {
        ChangeState(previousBehaviour);
    }

    public void ToggleNothingState()
    {
        ChangeState(new NothingBehaviour());
    }

}
