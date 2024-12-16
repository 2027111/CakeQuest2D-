using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContextClue : MonoBehaviour
{

    public Animator anim;


    private void Start()
    {
        anim = GetComponent<Animator>();
    }


    public void PopOut()
    {
        anim.SetTrigger("Popout");
    }


    public void PopIn(bool on)
    {
        anim.SetBool("Popin", on);

    }
}
