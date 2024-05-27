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
        InputManager.inputManager.OnSelectReleased += OnSelectReleased;
        InputManager.inputManager.OnReturnReleased += OnBackReleased;
        InputManager.inputManager.OnMovementPressed += OnNavigate;
        ShowControls();
    }


    public virtual void OnExit()
    {

        InputManager.inputManager.OnSelectPressed -= OnSelect;
        InputManager.inputManager.OnReturnPressed -= OnBack;
        InputManager.inputManager.OnSelectReleased -= OnSelectReleased;
        InputManager.inputManager.OnReturnReleased -= OnBackReleased;
        InputManager.inputManager.OnMovementPressed -= OnNavigate;
        BattleManager.Singleton.SetIndicationText("");
    }
    public virtual void OnSelectReleased()
    {
    }

    public virtual void OnBackReleased()
    {

    }
    public virtual void OnSelect()
    {
    }

    public virtual void OnBack()
    {

    }

    public virtual void ShowControls()
    {
        string controls = LanguageData.GetDataById("ControlScheme").GetValueByKey(this.GetType().ToString());

        battleManager.SetControlText(controls);
    } 

    public virtual void OnNavigate(Vector2 direction)
    {
    }
    public virtual void Handle()
    {

    }


}
