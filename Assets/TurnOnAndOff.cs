using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TurnOnAndOff : MonoBehaviour
{
    public bool TurnedOn;
    Animator anim;
    public UnityEvent OnTurnedOn;

    private void Start()
    {
        anim = GetComponent<Animator>();
        CheckOn();
    }

    private void CheckOn()
    {

        anim.SetBool("Opened", TurnedOn);
    }

    public void Turn()
    {
        if (TurnedOn == false)
        {

            anim.SetTrigger("Open");
            OnTurnedOn?.Invoke();
        }
        TurnedOn = !TurnedOn;
        CheckOn();
    }
}
