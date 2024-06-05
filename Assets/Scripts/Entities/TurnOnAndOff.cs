using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TurnOnAndOff : MonoBehaviour
{
    public bool TurnedOn;
    public BoolValue condition;
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
        anim.SetTrigger((TurnedOn?"Open":"Close"));
        if (TurnedOn)
        {
            OnTurnedOn?.Invoke();
        }
    }

    public void Turn()
    {
        if (condition)
        {
            if (!condition.RuntimeValue)
            {
                return;
            }
        }
        TurnedOn = !TurnedOn;
        CheckOn();
    }
}
