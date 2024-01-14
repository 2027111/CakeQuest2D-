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

        if (!player.heroParty.PartyMembers.Contains(characterObject))
        {
            player.heroParty.PartyMembers.Add(characterObject);
        }
    }



    public void RemoveFromParty()
    {

        if (player.heroParty.PartyMembers.Contains(characterObject))
        {
            player.heroParty.PartyMembers.Remove(characterObject);
        }
    }


    public void HealEntireParty()
    {
        player.heroParty.MainCharacter.Revitalize();
        foreach (CharacterObject obj in player.heroParty.PartyMembers)
        {
            obj.Revitalize();
        }
    }



}
