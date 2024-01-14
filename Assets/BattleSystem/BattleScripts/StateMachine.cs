using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine : MonoBehaviour
{

    public State CurrentState { get; set; }
    private State nextState;
    public LayerMask hitboxMask;


    // Start is called before the first frame update
    void Start()
    {
        if (CurrentState == null)
        {
            SetNextState(new EntranceState());
        }
    }
  /*  private void OnValidate()
    {
        if(CurrentState == null)
        {
            SetNextStateToMain();
        }
    }*/

    // Update is called once per frame
    void Update()
    {
        if(nextState != null)
        {
            SetState(nextState);
        }


        if (CurrentState != null)
        {
            CurrentState.OnUpdate();
        }
    }

    private void FixedUpdate()
    {

        if (CurrentState != null)
        {
            CurrentState.OnFixedUpdate();
        }

    }


    private void LateUpdate()
    {


        if (CurrentState != null)
        {
            CurrentState.OnLateUpdate();
        }

    }
    private void SetState(State nextState)
    {
        CurrentState?.OnExit();
        CurrentState = nextState;
        nextState.OnEnter(this); 
    }

    private void SetState(State nextState, AttackData data)
    {
        CurrentState?.OnExit();
        CurrentState = nextState;
        ((MeleeBaseState)nextState).OnEnter(this, data); 
    }

    public void SetNextStateToMain()
    {
        if (GetComponent<TeamComponent>().teamIndex == TeamIndex.Enemy)
        {
            SetNextState(new IdleCombatState());
        }
        else
        {
            SetNextState(new IdleCombatState());
        }
    }

    public void SetNextState(State _newState)
    {
        if(_newState != null)
        {
            SetState(_newState);
        }
    }
    public void SetNextState(State _newState, AttackData data)
    {
        if (_newState != null)
        {
            if(data != null)
            {
                if (typeof(MeleeBaseState).IsAssignableFrom(_newState.GetType()));
                {
                    SetState(_newState, data);
                }
            }
        }
    }


}
