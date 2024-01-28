using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State
{
    protected Animator animator;
    protected float time { get; set; }
    protected float fixedtime { get; set; }
    protected float latetime { get; set; }
    public StateMachine stateMachine;
    public BattleCharacter cc;

    
    public virtual void OnEnter(StateMachine _stateMachine)
    {
        stateMachine = _stateMachine;
        cc = stateMachine.GetComponent<BattleCharacter>();
        animator = _stateMachine.GetComponentInChildren<Animator>();
    }

    public virtual void OnFixedUpdate()
    {
        fixedtime += Time.deltaTime;
    }
    public virtual void OnLateUpdate()
    {
        latetime += Time.deltaTime;
    }

    public virtual void OnUpdate()
    {

        time += Time.deltaTime;
    }

    public virtual void OnExit()
    {

    }


    public virtual void DoAttack()
    {
        DoMoveset();
    }


    public virtual void DoSpecial()
    {
        DoMoveset(true);
    }

    public virtual void DoMoveset(bool special = false)
    {
        MoveData nextMove = cc.GetCurrentAttack(special);

        if (cc.canMove && cc.canAttack && nextMove && cc.entity.CheckManaCost(nextMove))
        {
            if (nextMove.grounded && !cc.groundTouch)
            {
                return;
            }

            stateMachine.SetNextState(new AttackState(), nextMove);
        }
    }



    public virtual void OnAttackInput()
    {
        MoveData attackData = cc.GetCurrentAttack();
        if(attackData && cc.canAttack)
        {
            stateMachine.SetNextState(new AttackState(), attackData);
        }
    }
}
