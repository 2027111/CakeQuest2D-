using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorController : MonoBehaviour
{
    Animator anim;



    private void Awake()
    {
        anim = GetComponentInChildren<Animator>();
    }

    public void Attack()
    {
        anim.speed = 1;
        anim?.SetTrigger("attack");
    }


    public void Hurt()
    {
        anim.speed = 1;
        anim?.SetTrigger("hurt");
    }
    public void Dodge()
    {
        anim.speed = 1;
        anim?.SetTrigger("dodge");
    }

    public void Die()
    {
        anim.speed = 1;
        anim?.SetTrigger("dead");
    }

    public void Concentrate()
    {
        anim.speed = 1;
        anim?.SetTrigger("concentrate");
    }
    public void ResetToIdle()
    {

        anim.speed = 1;
        anim?.SetTrigger("reset");
    }
    public void Block()
    {
        anim.speed = 1;
        anim?.SetBool("block", true);
    }

    public void Parry()
    {

        anim.speed = 1;
        anim?.SetTrigger("parry");
    }

    public AnimatorClipInfo GetCurrentAnim()
    {
            if (anim == null)
            {
                Debug.LogWarning("Animator component is missing.");
                return new AnimatorClipInfo();
            }

            AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(0);

            if (!stateInfo.IsName(""))
            {
                AnimatorClipInfo[] clipInfo = anim.GetCurrentAnimatorClipInfo(0);
                if (clipInfo.Length > 0)
                {
                    return clipInfo[0];
                }
            }

            Debug.LogWarning("No animation clips found or no animation is playing.");
            return new AnimatorClipInfo();
    }

    public float GetCurrentAnimTime()
    {
        if (anim == null)
        {
            Debug.LogWarning("Animator component is missing.");
            return 0;
        }

        AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(0);

        if (!stateInfo.IsName(""))
        {
            AnimatorClipInfo[] clipInfo = anim.GetCurrentAnimatorClipInfo(0);
            if (clipInfo.Length > 0)
            {
                float normalizedTime = stateInfo.normalizedTime;
                float clipLength = clipInfo[0].clip.length;
                return normalizedTime * clipLength;
            }
        }

        Debug.LogWarning("No animation clips found or no animation is playing.");
        return 0;
    }


    public void Revive()
    {
        anim.speed = 1;
        anim?.SetTrigger("revive");
    }



    public void Move(bool isMoving)
    {
        anim.speed = 1;
        anim?.SetBool("moving",isMoving);
    }

    public RuntimeAnimatorController GetController()
    {
        return anim.runtimeAnimatorController;
    }

    public void SetController(AnimatorOverrideController originalController)
    {
        anim.runtimeAnimatorController = originalController;
    }


    public float GetCurrentAnimLen()
    {
        return GetCurrentAnim().clip.length;
    }

    public void StopBlock()
    {
        anim.speed = 1;
        anim?.SetBool("block", false);
    }
}
