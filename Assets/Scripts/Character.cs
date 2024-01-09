using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{


    CharacterBehaviour playerBehaviour;
    Movement playerMovement;
    public CharacterInventory inventory;
    public bool canInteract = false;
    public event Action OnInteractEvent;
    public Party heroParty;

    // Start is called before the first frame update
    private void Awake()
    {
        playerMovement = GetComponent<Movement>();
    }
    void Start()
    {
        TogglePlayableState(); 
    }




    // Update is called once per frame
    void Update()
    {
        
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

    public void LookToward(Vector2 direction)
    {
        playerMovement.LookAt(direction);
    }



    public void LookAt(GameObject target)
    {

        LookToward((target.transform.position - transform.position).normalized);
    }

    public void SetPosition(Vector3 newPosition)
    {
        if (!playerMovement)
        {
            playerMovement = GetComponent<Movement>();
        }


        playerMovement?.SetPosition(newPosition);
    }

    public void ChangeState(CharacterBehaviour newBehaviour)
    {
        playerBehaviour?.OnExit();
        playerBehaviour = newBehaviour;
        newBehaviour.OnEnter(this);
    }


    public CharacterBehaviour GetCurrentBehaviour()
    {
        return playerBehaviour;
    }


    public void InteractPressed()
    {
        if (canInteract)
        {
            OnInteractEvent?.Invoke();
        }
    }

    private void FixedUpdate()
    {
        playerBehaviour.Handle();
    }



    public void TogglePlayableState()
    {
        if (GetComponent<PlayerInputController>())
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
}
