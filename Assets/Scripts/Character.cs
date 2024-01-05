using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{


    CharacterBehaviour playerBehaviour;
    Movement playerMovement;
    public Inventory inventory;
    public bool canInteract = false;
    public event Action OnInteractEvent;

    // Start is called before the first frame update
    private void Awake()
    {
        playerMovement = GetComponent<Movement>();
    }
    void Start()
    {
        ChangeState(new PlayerControlsBehaviour());
    }




    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddToInventory(Item content, int amount = 1)
    {
        if (inventory)
        {
            if(content.itemName == "pesso")
            {
                inventory.AddMoney(amount);
            }
            else
            {
                for(int i = 0; i < amount; i++)
                {
                    inventory.items.Add(content);
                }
            }
        }
    }

    public bool HasObject(Item content, int amount = 1)
    {
        List<Item> occurrences = inventory.items.FindAll(x => x == content);
        if (occurrences.Count < amount)
        {
            return false;
        }
        else
        {
            return true;
        }
    }


    public int AmountObject(Item content)
    {
        List<Item> occurrences = inventory.items.FindAll(x => x == content);
        return occurrences.Count;
        
    }
    public bool RemoveFromInventory(Item content, int amount = 1)
    {

        List<Item> occurrences = inventory.items.FindAll(x => x == content);
        if(occurrences.Count < amount)
        {
            return false;
        }
        for (int i = 0; i < amount; i++)
        {
            inventory.items.Remove(content);
        }
        return true; ;
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


}
