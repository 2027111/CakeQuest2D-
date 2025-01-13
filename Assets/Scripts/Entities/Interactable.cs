using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public enum ActionIndicatorType
{
    interact,
    open,
    talk
}
public class Interactable : MonoBehaviour
{


    public UnityEvent interactionEvent;
    public UnityEvent contactEvent;
    public UnityEvent contactEndEvent;

    GameObject player;

    [SerializeField] GameObject InteractionIndicator;
    public ActionIndicatorType ActionIndicatorKey = ActionIndicatorType.interact;

    private void Start()
    {
        ContextClue(false);
    }

    public void InteractionInvoke()
    {
        RestoreIndication(false);
        interactionEvent?.Invoke();
    }



    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (enabled)
        {

            if (collision.CompareTag("Player"))
            {
                player = collision.gameObject;
                if (player.GetComponent<Character>().GetCurrentBehaviour() is PlayerControlsBehaviour)
                {


                    ManageContactEvent(contactEvent);
                    if (player.GetComponent<Character>().CanInteraction())
                    {
                        player.GetComponent<Character>().SetInteraction(false);
                        ManageInteraction(player, true);
                    }
                    else
                    {
                        player = null;
                    }
                }

            }
        }
    }

    public void RestoreIndication(bool restore)
    {
            if(player != null)
            {
                ContextClue(restore);
                UICanvas.Singleton.SetActionIndicatorUI(restore, ActionIndicatorKey.ToString());
            }
       
    }

    public void ManageInteraction(GameObject character, bool add)
    {
        if (interactionEvent.GetPersistentEventCount() > 0)
        {
            RestoreIndication(add);
            if (add)
            {
                player.GetComponent<Controller>().OnSelectPressed += InteractionInvoke;
            }
            else
            {

                player.GetComponent<Controller>().OnSelectPressed -= InteractionInvoke;
            }
        }
    }

    public void ManageContactEvent(UnityEvent currentevent)
    {
        if (currentevent.GetPersistentEventCount() > 0)
        {
            currentevent?.Invoke();
        }
    }


    private void OnTriggerExit2D(Collider2D collision)
    {
        if (enabled)
        {
            if (collision.CompareTag("Player"))
            {
                if (player != null)
                {
                    player.GetComponent<Character>().SetInteraction(true);
                    ManageInteraction(player, false);
                    ManageContactEvent(contactEndEvent);
                    ContextClue(false);

                    player = null;
                }
            }

        }
    }

    private void OnDisable()
    {
        if (player != null)
        {
            player.GetComponent<Character>().SetInteraction(true);
            ManageInteraction(player, false);
            ManageContactEvent(contactEndEvent);
            ContextClue(false);

            player = null;
        }
    }

    public void ContextClue(bool on)
    {
        if (InteractionIndicator)
        {
            InteractionIndicator?.GetComponent<ContextClue>().PopIn(on);
        }
    }



    public void Disable()
    {
        if (player)
        {
            ManageInteraction(player, false);
            player.GetComponent<Character>().SetInteraction(true);
            ContextClue(false);
            player = null;
        }


        this.enabled = false;
    }
}
