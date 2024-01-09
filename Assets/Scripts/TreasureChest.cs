using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class TreasureChest : DialogueStarterObject
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
                anim?.SetBool("Opened", isOpen);
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

        if(dialogueLines.Length > 0)
        {




        if (!started)
        {
            LineInfo[] Lines = dialogueLines;
            started = true;
            if (locked)
            {
                Action callback = DialogueOver;
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
                DialogueBox.Singleton.StartDialogue(GetFormattedLines(this, Lines), callback, player.gameObject, gameObject);

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
            LineInfo[] Lines = dialogueLines;
            isOpen = true;
            storedOpen.RuntimeValue = true;
            player.AddToInventory(content, amount);
            if (anim)
            {

                anim.SetTrigger("Open");
            }
            CheckOpen();
            Action temp = DialogueOver; 
            DialogueBox.Singleton.StartDialogue(GetFormattedLines(this, Lines), DialogueOver, player.gameObject, gameObject);



    }

    public override void DialogueOver()
    {
        base.DialogueOver();
    }


    
}
