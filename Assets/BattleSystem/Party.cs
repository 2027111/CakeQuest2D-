using Newtonsoft.Json.Linq;
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

    public override string GetJsonData()
    {

        var jsonObject = JObject.Parse(base.GetJsonData()); // Start with base class data


        jsonObject["party"] = JArray.FromObject(GetStringifiedParty()); // Adding additional data

        return jsonObject.ToString();



    }


    public List<string> GetStringifiedParty()
    {
        List<string> partyIds = new List<string>();
        foreach(CharacterObject co in PartyMembers)
        {
            partyIds.Add(co.UID);
        }
        return partyIds;
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

        yield return null;
    }

    public void SetParty(List<CharacterObject> fightParty)
    {
        PartyMembers.Clear();
        PartyMembers.AddRange(fightParty);
    }
}
