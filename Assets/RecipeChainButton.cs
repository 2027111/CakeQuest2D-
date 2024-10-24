using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RecipeChainButton : ChoiceMenuButton
{
    [SerializeField] string logoPath = "Sprites/logo_element_";
    [SerializeField] Image AttackLogo;
    Command command;
    public void SetCommand(Command command)
    {
        this.command = command;

        
        Sprite attackSprite = Resources.Load<Sprite>(logoPath + command.GetElement());
        if (attackSprite)
        {
            AttackLogo.sprite = attackSprite;
            AttackLogo.transform.up = Vector2.up;
        }
    }

    public Command GetCommand()
    {
        return command;
    }
}
