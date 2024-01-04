using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterBehaviour
{

    public Character character;
    public Movement movement;
    protected float time;

    public CharacterBehaviour()
    {

    }



    public virtual void OnEnter(Character player)
    {
        this.character = player;
        movement = player.GetComponent<Movement>();
    }



    public virtual void Handle()
    {
        time += Time.deltaTime;

    }


    public virtual void OnExit()
    {

    }





}
