using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        FadeScreen.Singleton?.OnFadingStart.AddListener(delegate { ChangeState(new NothingBehaviour()); });
    }


    private void OnDisable()
    {

        FadeScreen.Singleton?.OnFadingStart.RemoveListener(delegate { ChangeState(new NothingBehaviour()); });
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

    public bool InteractContains(Action interact)
    {
        if (OnInteractEvent == null)
        {
            return false;
        }
        return OnInteractEvent.GetInvocationList().Contains(interact);
    }
}
