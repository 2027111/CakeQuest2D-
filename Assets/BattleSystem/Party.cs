using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu]
public class Party : SavableObject
{

    public List<CharacterObject> PartyMembers = new List<CharacterObject>();
    public UnityEvent OnAddToParty;
    public async override void ApplyData(SavableObject tempCopy)
    {
        PartyMembers = new List<CharacterObject>();

        AddCharactersToParty((tempCopy as Party).PartyMembers);
        //GameSaveManager.Singleton.StartCoroutine(AddLoadedCharactersToParty((tempCopy as Party).PartyMembers));
        base.ApplyData(tempCopy);
    }

    public void AddCharactersToParty(List<CharacterObject> loadedParty)
    {

        PartyMembers = new List<CharacterObject>();

        foreach (CharacterObject item in loadedParty)
        {
            if(ObjectLibrary.Library.TryGetValue(item.UID, out SavableObject value))
            {
                PartyMembers.Add(value as CharacterObject);
            }
        }

    }

    public IEnumerator AddLoadedCharactersToParty(List<CharacterObject> loadedParty)
    {

        PartyMembers = new List<CharacterObject>();

        foreach (CharacterObject item in loadedParty)
        {
            ResourceRequest request = Resources.LoadAsync<CharacterObject>($"CharacterFolder/{item.name}");
            while (!request.isDone)
            {
                yield return null;
            }
            CharacterObject loadedChar = request.asset as CharacterObject;
            PartyMembers.Add(loadedChar);
            yield return null;
        }

        Debug.Log("Current party " + name + " Has Member count : " + PartyMembers.Count);
        yield return null;
    }

    public void SetParty(List<CharacterObject> fightParty)
    {
        PartyMembers.Clear();
        PartyMembers.AddRange(fightParty);
    }
}
