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
    public LineInfo[] LockedLines;
    public LineInfo[] SuccessLines;
    public BoolValue storedOpen;
    Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Character>();
        isOpen = storedOpen.RuntimeValue;
        CheckOpen();
        

    }


    protected void CheckOpen()
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
            LineInfo[] Lines = dialogue.dialogueLines;
            started = true;

            Dialogue newDialogue = new Dialogue(dialogue);
            if (locked)
            {
                UnityAction callback = DialogueOver;
                if (CheckRequirement())
                {


                    Lines = SuccessLines;
               
                    if (consumesRequirement)
                        {
                        player.RemoveFromInventory(lockRequirement, requirementAmount);
                        }
                    Unlock();
                    callback = RequirementMetEvent;
                }
                else
                {
                    Lines = LockedLines;
                }
                newDialogue.dialogueLines = GetFormattedLines(this, Lines);
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
        LineInfo[] Lines = newDialogue.dialogueLines;
        Debug.Log(Lines.Length);
        newDialogue.dialogueLines = GetFormattedLines(this, Lines);
        newDialogue.OnOverEvent.RemoveAllListeners();
        newDialogue.OnOverEvent.AddListener(DialogueOver);
        DialogueBox.Singleton.StartDialogue(newDialogue, player.gameObject, gameObject);



    }

    public override void DialogueOver()
    {
        Debug.Log("Lol");
        base.DialogueOver();
    }


    
}
