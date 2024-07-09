using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NothingBehaviour : CharacterBehaviour
{


    
    public NothingBehaviour() : base()
    {
    }



    public override void OnEnter(Character player)
    {
        base.OnEnter(player);
        if (player.gameObject.CompareTag("Player"))
        {
            UICanvas.TurnBordersOn(false);
        }
    }



    public override void Handle()
    {
        
    }


    public override void OnExit()
    {
    }







}
