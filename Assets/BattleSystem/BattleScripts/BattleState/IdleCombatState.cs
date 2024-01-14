using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleCombatState : State
{
    private Collision coll;
    public Rigidbody2D rb;


    public override void OnEnter(StateMachine _stateMachine)
    {
        base.OnEnter(_stateMachine);
        coll = stateMachine.GetComponent<Collision>();
        rb = stateMachine.GetComponent<Rigidbody2D>();
        
        cc.ResetMovement();
        cc.OnAttackPressed += DoAttack;
        cc.OnSpecialPressed += DoSpecial;
        cc.OnJumpPressed += DoJump;
    }


    public override void OnExit()
    {
        base.OnExit();
        cc.OnAttackPressed -= DoAttack;
        cc.OnSpecialPressed -= DoSpecial;
        cc.OnJumpPressed -= DoJump;
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
        float x = cc.direction.x;
        float y = cc.direction.y;



        if(Time.timeScale != 0)
        {

        Vector2 dir = new Vector2(x, y);
        cc.Walk(dir);
        






      

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
        if (rb.velocity.y < 0)
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (cc.fallMultiplier - 1) * Time.deltaTime;
        }
        else if (rb.velocity.y > 0 && !cc.jump)
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (cc.lowJumpMultiplier - 1) * Time.deltaTime;
        }

        }

        animator.SetBool("Grounded", coll.onGround && rb.velocity.y <= 0.1);
        animator.SetFloat("MovementSpeed", Mathf.Abs(rb.velocity.x) > 0.3f?1:0);
        animator.SetFloat("YVelocity", rb.velocity.y);


    }
    private void DoJump()
    {
        if (coll.onGround)
        {

            cc.Jump(Vector2.up, false);
            cc.doubleJumpCoolDownAmount = cc.doubleJumpCoolDown;
        }
        else if (cc.doubleJumpAmount > 0 && cc.doubleJumpCoolDownAmount <= 0)
        {

            cc.Jump(Vector2.up, false);
            cc.doubleJumpAmount--;
            cc.doubleJumpCoolDownAmount = cc.doubleJumpCoolDown;
        }
    }


    public void DoAttack()
    {
        Debug.Log("Attack Input");
        if (cc.canMove && cc.canAttack && cc.GetCurrentAttack() && cc.entity.CheckManaCost(cc.GetCurrentAttack()))
        {
            stateMachine.SetNextState(new MeleeBaseState(), cc.GetCurrentAttack());
        }
    }


    public void DoSpecial()
    {
        Debug.Log("Special Input");
        if (cc.canMove && cc.canAttack && cc.GetCurrentAttack(true) && cc.entity.CheckManaCost(cc.GetCurrentAttack(true)))
        {
            stateMachine.SetNextState(new MeleeBaseState(), cc.GetCurrentAttack(true));
        }
    }



    void RigidbodyDrag(float x)
    {
        rb.drag = x;
    }

}

