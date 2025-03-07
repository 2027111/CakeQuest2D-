using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class TreasureChest : BranchingDialogueStarterObject
{






    public InventoryItem content;
    public int amount = 1;
    public bool OnlyOnce = true;
    public bool isOpen;
    public bool locked;
    public InventoryItem lockRequirement;
    public int requirementAmount = 1;
    public bool consumesRequirement;
    public DSDialogueSO LockedDialogue;
    public DSDialogueSO SuccessDialogue;
    public BoolValue storedOpen;


    public UnityEvent OnIsOpen;



    [SerializeField] private int selectedSuccessDialogueIndex = 0;
    [SerializeField] private int selectedLockedDialogueIndex = 0;


    private void Start()
    {
        isOpen = storedOpen.RuntimeValue;
        CheckOpen(true);


    }


    protected void CheckOpen(bool forceOpen = false)
    {
        if (isOpen || content == null)
        {

            OnIsOpen?.Invoke();
            Interactable inter = GetComponent<Interactable>();
            if (inter && OnlyOnce)
            {
                inter.Disable();
            }
        }
    }


    public override void DialogueAction()
    {

        if (CheckLines())
        {




            if (!started)
            {
                Dialogue newDialogue = new Dialogue(dialogue, DialogueEvents, DialogueAction);
                started = true;
                if (locked)
                {
                    UnityAction callback = DialogueOver;
                    if (CheckRequirement())
                    {

                        newDialogue = new Dialogue(SuccessDialogue, DialogueEvents, DialogueAction);

                        if (consumesRequirement)
                        {
                            Character.Player.RemoveFromInventory(lockRequirement, requirementAmount);
                        }
                        Unlock();
                        callback = RequirementMetEvent;
                    }
                    else
                    {
                        newDialogue = new Dialogue(LockedDialogue, DialogueEvents, DialogueAction);
                    }
                    newDialogue.SetSource(this);
                    newDialogue.OnOverEvent.RemoveAllListeners();
                    newDialogue.OnOverEvent.AddListener(callback);
                    UICanvas.StartDialogue(newDialogue, Character.Player.gameObject, gameObject);

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
        return Character.Player.HasObject(lockRequirement, requirementAmount);
    }
    public void RequirementMetEvent()
    {
        isOpen = true;
        storedOpen.RuntimeValue = true;
        Character.Player.AddToInventory(content, amount);
        CheckOpen();
        Dialogue newDialogue = new Dialogue(dialogue, DialogueEvents, DialogueAction); ;
        newDialogue.SetSource(this);
        newDialogue.OnOverEvent.RemoveAllListeners();
        newDialogue.OnOverEvent.AddListener(DialogueOver);
        UICanvas.StartDialogue(newDialogue, Character.Player.gameObject, gameObject);



    }

    public override void DialogueOver()
    {
        base.DialogueOver();
    }



}
