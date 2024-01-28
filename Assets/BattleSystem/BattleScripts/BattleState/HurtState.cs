using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HurtState : MeleeBaseState
{


    public HurtState(float duration) : base()
    {
        this.duration = duration;
    }
    public HurtState() : base()
    {
        this.duration = Random.Range(0.2f, 0.3f);
    }
    public override void OnEnter(StateMachine _stateMachine)
    {
        base.OnEnter(_stateMachine);
        animator.SetTrigger("Hurt");


        AudioClip HurtClip = stateMachine.GetComponent<Entity>().characterObject.GetHurtVoiceClip();
        PlayHurtClip(HurtClip);
        stateMachine.GetComponent<Effector2D>().enabled = false;
        //stateMachine.StartCoroutine(HitPause());
    }


    public void PlayHurtClip(AudioClip HurtClip)
    {
        cc.PlayVoiceclip(HurtClip);
    }


    public override void OnUpdate()
    {

        if (fixedtime > duration)
        {
            stateMachine.SetNextStateToMain();

        }
    }

    public override void OnExit()
    {
        stateMachine.GetComponent<Effector2D>().enabled = true;
        base.OnExit();

    }

    public IEnumerator HitPause()
    {
        Time.timeScale = 0.5f;
        yield return new WaitForSecondsRealtime(.2f);
        Time.timeScale = 1;
        yield return null;
    }
}
