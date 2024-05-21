using System.Collections;
using System.Collections.Generic;
using UnityEngine;






[CreateAssetMenu]
public class Party : ScriptableObject
{

    public CharacterObject MainCharacter;
    public List<CharacterObject> PartyMembers = new List<CharacterObject>();



}
