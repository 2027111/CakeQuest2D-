using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BattleState
{
    protected BattleManager battleManager;
    
    public virtual void OnEnter(BattleManager _battleManager)
    {
        battleManager = _battleManager;
        InputManager.inputManager.OnSelectPressed += OnSelect;
        InputManager.inputManager.OnReturnPressed += OnBack;
        InputManager.inputManager.OnMovement += OnNavigate;
    }


    public virtual void OnExit()
    {

        InputManager.inputManager.OnSelectPressed -= OnSelect;
        InputManager.inputManager.OnReturnPressed -= OnBack;
        InputManager.inputManager.OnMovement -= OnNavigate;
    }

    public virtual void OnSelect()
    {

        Debug.Log("Select");
    }

    public virtual void OnBack()
    {

        Debug.Log("Return");
    }

    public virtual void OnNavigate(Vector2 direction)
    {
        Debug.Log("Navigate");
    }
    public virtual void Handle()
    {

    }
}
