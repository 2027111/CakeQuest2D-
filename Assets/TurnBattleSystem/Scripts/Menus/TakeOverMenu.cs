using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakeOverMenu : ChoiceMenu
{
    public List<BattleCharacter> possibleTakeOvers;
    public GameObject takeOverButtonPrefab;



    public void GiveTakeOvers(List<BattleCharacter> takeOvers, PerformActionState state)
    {
        if(takeOvers.Count == 0)
        {
            DestroyMenu();
        }
        else
        {
            int index = Random.Range(0, takeOvers.Count);
            GameObject button = Instantiate(takeOverButtonPrefab, this.transform);
            button.GetComponent<TakeOverButton>().SetBattleCharacter(takeOvers[index]);
            button.GetComponent<TakeOverButton>().OnSelected.AddListener(button.GetComponent<TakeOverButton>().Confirm);
            button.GetComponent<TakeOverButton>().OnSelected.AddListener(delegate { state.ForceTurn(takeOvers[index]); });
            button.GetComponent<TakeOverButton>().OnSelected.AddListener(button.GetComponent<TakeOverButton>().Over);
            button.GetComponent<TakeOverButton>().OnSelected.AddListener(DestroyMenu);

            buttons.Add(button.gameObject);
            DefaultSelect();
        }
    }

    public void DestroyMenu()
    {
        foreach(GameObject button in buttons)
        {
            button.GetComponent<TakeOverButton>().Over();
        }
        Destroy(gameObject);
    }
}
