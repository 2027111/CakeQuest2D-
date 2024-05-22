using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillMenu : ChoiceMenu
{

    public GameObject ButtonPrefab;



    public void ResetMenu()
    {
        foreach (Transform transform in transform)
        {
            Destroy(transform.gameObject);
        }
    }

    public void AddButtons(List<Attack> attacks)
    {
        foreach (Attack obj in attacks)
        {

            ButtonObject button = Instantiate(ButtonPrefab, transform).GetComponent<ButtonObject>();
            button.SetText(obj.ToString());
            button.GetComponent<ChoiceMenuButton>().OnSelected.AddListener(TriggerSkill);
            button.GetComponent<SkillButton>().SetSkill(obj);
            buttons.Add(button.gameObject);

        }
        DefaultSelect();
    }

    public void EnableGridLayout(bool on)
    {
        GetComponent<GridLayoutGroup>().enabled = on;
    }
    public void TriggerSkill()
    {
        Attack attack = SelectedButton.GetComponent<SkillButton>().storedSkill;

        Command attackCommand = attack.GetCommandType();
        attackCommand.SetSource(BattleManager.Singleton.GetActor());

        if (attack.manaCost > BattleManager.Singleton.GetActor().Mana  || BattleManager.Singleton.GetPossibleTarget(attackCommand).Count == 0)
        {
            SelectedButton.GetComponent<ChoiceMenuButton>().SelectFailed();
        }
        else
        {
            BattleManager.Singleton.GetActor().currentCommand = attackCommand;
            BattleManager.Singleton.ChangeState(new ChoosingTargetState());
        }
    }
}
