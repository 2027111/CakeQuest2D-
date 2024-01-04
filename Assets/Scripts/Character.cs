using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{


    CharacterBehaviour playerBehaviour;
    Movement playerMovement;
    public bool canInteract = false;
    public event Action OnInteractEvent;

    // Start is called before the first frame update
    void Start()
    {
        playerMovement = GetComponent<Movement>();
        ChangeState(new PlayerControlsBehaviour());
    }




    // Update is called once per frame
    void Update()
    {
        
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
