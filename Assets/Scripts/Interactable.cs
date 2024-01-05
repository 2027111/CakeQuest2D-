using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{


    public UnityEvent interactionEvent;
    public UnityEvent contactEvent;



    
    GameObject player;

    [SerializeField] GameObject InteractionIndicator;


    private void Start()
    {
        ContextClue(false);
    }

    public void InteractionInvoke()
    {
        interactionEvent?.Invoke();
    }

    public void ContactInvoke()
    {
        contactEvent?.Invoke();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            player = collision.gameObject;
            ManageInteraction(player, true);
            ContextClue(true);
        }
    }
    public void ManageInteraction(GameObject character, bool add)
    {
        if(interactionEvent.GetPersistentEventCount() > 0)
        {
            if (add)
            {

                player.GetComponent<Character>().OnInteractEvent += InteractionInvoke;
            }
            else
            {

                player.GetComponent<Character>().OnInteractEvent -= InteractionInvoke;
            }
        }
    }
    public void ManageContact()
    {
        if(contactEvent.GetPersistentEventCount() > 0)
        {
            ContactInvoke();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            ManageInteraction(player, false);
            ContextClue(false);
            
            player = null;
        }
    }


    public void ContextClue(bool on)
    {
        if (InteractionIndicator)
        {
            InteractionIndicator?.SetActive(on);
        }
    }

    public void Disable()
    {
        if (player)
        {
            player.GetComponent<Character>().OnInteractEvent -= InteractionInvoke;
            ContextClue(false);
            player = null;
        }



        Destroy(this);
    }
}
