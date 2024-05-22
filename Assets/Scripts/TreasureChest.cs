using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class TreasureChest : NewDialogueStarterObject
{






    public InventoryItem content;
    public int amount = 1;
    public bool OnlyOnce = true;
    public bool isOpen;
    public bool locked;
    public InventoryItem lockRequirement;
    public int requirementAmount = 1;
    public bool consumesRequirement;
    public Dialogue LockedDialogue;
    public Dialogue SuccessDialogue;
    public BoolValue storedOpen;
    Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Character>();
        isOpen = storedOpen.RuntimeValue;
        CheckOpen(true);
        

    }


    protected void CheckOpen(bool forceOpen = false)
    {
        if (isOpen || content == null)
        {
            if (anim)
            {
                anim.SetBool("Opened", isOpen);
                anim.SetTrigger((isOpen ? "Open" : "Close"));
            }
            Interactable inter = GetComponent<Interactable>();
            if (inter && OnlyOnce) 
            {
                inter.Disable();
            }
        }
    }


    public override void DialogueAction()
    {

        if(CheckLines())
        {




        if (!started)
        {
            Dialogue newDialogue = new Dialogue(dialogue);
            started = true;
            if (locked)
            {
                UnityAction callback = DialogueOver;
                if (CheckRequirement())
                {

                    newDialogue = new Dialogue(SuccessDialogue);
               
                    if (consumesRequirement)
                        {
                        player.RemoveFromInventory(lockRequirement, requirementAmount);
                        }
                    Unlock();
                    callback = RequirementMetEvent;
                }
                else
                    {
                    newDialogue = new Dialogue(LockedDialogue);
                }
                newDialogue.SetSource(this.gameObject);
                newDialogue.OnOverEvent.RemoveAllListeners();
                newDialogue.OnOverEvent.AddListener(callback);
                DialogueBox.Singleton.StartDialogue(newDialogue, player.gameObject, gameObject);

            }
            else
            {
                RequirementMetEvent();
            }


            }
        }


    }


    public void Unlock()
    {

        locked = false;
    }

    public void StartDialogue()
    {

    }


    public bool CheckRequirement()
    {
        return player.HasObject(lockRequirement, requirementAmount);
    }
    public void RequirementMetEvent()
    {
        isOpen = true;
        storedOpen.RuntimeValue = true;
        player.AddToInventory(content, amount);
        if (anim)
        {

            anim.SetTrigger("Open");
        }
        CheckOpen();
        Dialogue newDialogue = new Dialogue(dialogue);
        newDialogue.SetSource(this.gameObject);
        newDialogue.OnOverEvent.RemoveAllListeners();
        newDialogue.OnOverEvent.AddListener(DialogueOver);
        DialogueBox.Singleton.StartDialogue(newDialogue, player.gameObject, gameObject);



    }

    public override void DialogueOver()
    {
        base.DialogueOver();
    }


    
}
