using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecipeChainButton : ChoiceMenuButton
{

    Command command;
    public void SetCommand(Command command)
    {
        this.command = command;
    }

}
