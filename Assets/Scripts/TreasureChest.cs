using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreasureChest : DialogueStarterObject
{
    public Item content;
    [SerializeField] int amount = 1;
    public bool isOpen;
    public bool locked;
    public Item lockRequirement;
    public int requirementAmount = 1;
    public bool consumesRequirement;
    public BoolValue storedOpen;
    Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Character>();
        isOpen = storedOpen.RuntimeValue;
        CheckOpen();
        

    }

    private void CheckOpen()
    {
        if (isOpen)
        {
            anim.SetBool("Opened", isOpen);
            Interactable inter = GetComponent<Interactable>();
            if (inter)
            {
                inter.Disable();
            }
        }
    }

    public override void DialogueAction()
    {
        if (!started)
        {
        started = true;
        string[] vs;
        if (locked)
        {
            Action temp = DialogueOver;
            if (player.HasObject(lockRequirement, requirementAmount))
            {


                vs = new string[] { $"Le coffre est verouillé...", $"Mais vous avez le nombre de {content.itemName} nécéssaire! ({requirementAmount})"};
               
                if (consumesRequirement)
                {
                    player.RemoveFromInventory(lockRequirement, requirementAmount);
                }
                locked = false;
                temp = OpenChest;
            }
            else
            {
                vs = new string[] { $"Le coffre est verouillé...", $"Vous n'avez pas le nombre de {content.itemName} nécéssaire... ({player.AmountObject(lockRequirement)}/{requirementAmount})" };
            }
            DialogueBox.Singleton.StartDialogue(vs, temp, player.gameObject, gameObject);

            }
            else
            {
                OpenChest();
            }



        




        }

    }
    public void OpenChest()
    {
            string[] vs;
            vs = new string[] { $"Vous avez trouvé {amount} {content.itemName + (amount > 1 ? "s" : "")}" };
            isOpen = true;
            storedOpen.RuntimeValue = true;
            player.AddToInventory(content, amount);
            anim.SetTrigger("Open");
            CheckOpen();
            Action temp = DialogueOver;
            DialogueBox.Singleton.StartDialogue(vs, temp, player.gameObject, gameObject);

        
    }

    public override void DialogueOver()
    {
        base.DialogueOver();
    }


    
}
