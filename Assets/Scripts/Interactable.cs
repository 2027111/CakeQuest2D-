using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{


    public UnityEvent interactionEvent;

    GameObject player;

    [SerializeField] GameObject InteractionIndicator;




    public void InteractionInvoke()
    {
        interactionEvent.Invoke();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Player Entered zone");
        if (collision.CompareTag("Player"))
        {
            player = collision.gameObject;
            player.GetComponent<Character>().OnInteractEvent += InteractionInvoke;
            if (InteractionIndicator)
            {
                InteractionIndicator?.SetActive(true);
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        Debug.Log("Player left zone");
        if (collision.CompareTag("Player"))
        {
            player.GetComponent<Character>().OnInteractEvent -= InteractionInvoke;
            if (InteractionIndicator)
            {
                InteractionIndicator?.SetActive(false);
            }
            player = null;
        }
    }





}
