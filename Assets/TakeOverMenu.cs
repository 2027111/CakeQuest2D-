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
            foreach(BattleCharacter bc in takeOvers)
            {
                GameObject button = Instantiate(takeOverButtonPrefab, transform);
                button.GetComponent<TakeOverButton>().SetBattleCharacter(bc);
                button.GetComponent<TakeOverButton>().OnSelected.AddListener(delegate { state.ForceTurn(bc); });
                button.GetComponent<TakeOverButton>().OnSelected.AddListener(DestroyMenu);

                buttons.Add(button.gameObject);
            }
            DefaultSelect();
        }
    }

    public void DestroyMenu()
    {

        Destroy(gameObject);
    }
}
