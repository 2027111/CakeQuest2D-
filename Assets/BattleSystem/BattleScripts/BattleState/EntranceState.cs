using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EntranceState : State
{

    private Collision coll;
    public Rigidbody2D rb;



    public override void OnEnter(StateMachine _stateMachine)
    {
        base.OnEnter(_stateMachine);
        coll = stateMachine.GetComponent<Collision>();
        rb = stateMachine.GetComponent<Rigidbody2D>();
        cc.canMove = false;
        rb.velocity = Vector3.zero;
        //rb.isKinematic = true;
        cc.gameObject.layer = 13;
        animator.SetBool("IsAttacking", false);
    }

    public override void OnUpdate()
    {

        if (rb.velocity.y < 0)
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (cc.fallMultiplier - 1) * Time.deltaTime;
        }
        else if (rb.velocity.y > 0 && !cc.jump)
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (cc.lowJumpMultiplier - 1) * Time.deltaTime;
        }


        animator.SetBool("Grounded", coll.onGround && rb.velocity.y <= 0.1);
        animator.SetFloat("MovementSpeed", Mathf.Abs(rb.velocity.x) > 0.3f ? 1 : 0);
        animator.SetFloat("YVelocity", rb.velocity.y);
    }

    public override void OnExit()
    {
        base.OnExit();
        animator.updateMode = AnimatorUpdateMode.Normal;

        cc.canMove = true;
        cc.gameObject.layer = 9;
        animator.SetBool("IsAttacking", false);
        animator.Rebind();
        animator.Update(0);
    }

}