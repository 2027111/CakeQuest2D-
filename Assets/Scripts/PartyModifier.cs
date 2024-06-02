using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartyModifier : MonoBehaviour
{
    public Character player;
    public CharacterObject characterObject;




    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Character>();


    }

    public void AddToParty()
    {
        if(characterObject != null)
        {


        if (!player.heroParty.PartyMembers.Contains(characterObject))
        {
            player.heroParty.PartyMembers.Add(characterObject);
                characterObject.InParty.SetRuntime();
                player.heroParty.OnAddToParty?.Invoke();
            }
        }
    }



    public void RemoveFromParty()
    {

        if (characterObject != null)
        {

            if (player.heroParty.PartyMembers.Contains(characterObject))
        {
            player.heroParty.PartyMembers.Remove(characterObject);
                characterObject.InParty.ResetRuntime();
                player.heroParty.OnAddToParty?.Invoke();
            }
        }
    }


    public void HealEntireParty()
    {
        foreach (CharacterObject obj in player.heroParty.PartyMembers)
        {
            obj.Revitalize();
        }
        UICanvas.UpdatePartyList();
    }



}
