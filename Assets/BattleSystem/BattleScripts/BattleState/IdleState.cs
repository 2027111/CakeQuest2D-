using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : State
{
    private Collision coll;
    [HideInInspector]
    public Rigidbody2D rb;

    public override void OnEnter(StateMachine _stateMachine)
    {
        base.OnEnter(_stateMachine);
        coll = stateMachine.GetComponent<Collision>();
        rb = stateMachine.GetComponent<Rigidbody2D>();
    }

    public override void OnExit()
    {
        base.OnExit();
        animator.SetFloat("MovementSpeed", 0);
    }

    public override void OnFixedUpdate()
    {
        base.OnFixedUpdate();
    }

    public override void OnLateUpdate()
    {
        base.OnLateUpdate();
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        int x =0, y = 0;
        Vector2 dir = new Vector2(x, y);

        //cc.Walk(dir);


        if (coll.onGround)
        {
            //cc.GetComponent<BetterJumping>().enabled = true;
        }


        if (coll.onGround && !cc.groundTouch)
        {
            cc.groundTouch = true;
        }

        if (!coll.onGround && cc.groundTouch)
        {
            cc.groundTouch = false;
        }

        if (x > 0 && coll.onGround)
        {
            cc.side = 1;
            cc.Flip(cc.side);
        }
        if (x < 0 && coll.onGround)
        {
            cc.side = -1;
            cc.Flip(cc.side);
        }

        animator.SetBool("Grounded", coll.onGround && rb.velocity.y <= 0);
        animator.SetFloat("MovementSpeed", Mathf.Abs(rb.velocity.x) > 0.2f?1:0);
        animator.SetFloat("YVelocity", rb.velocity.y);
    }


 

 


    void RigidbodyDrag(float x)
    {
        rb.drag = x;
    }

}

