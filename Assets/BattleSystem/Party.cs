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

    public override string GetJsonData()
    {

        var jsonObject = JObject.Parse(base.GetJsonData()); // Start with base class data


        jsonObject["party"] = JArray.FromObject(GetStringifiedParty()); // Adding additional data

        return jsonObject.ToString();



    }

    public List<string> GetStringifiedParty()
    {
        List<string> partyIds = new List<string>();
        foreach (CharacterObject co in PartyMembers)
        {
            partyIds.Add(co.UID);
        }
        return partyIds;
    }


    public override void ApplyJsonData(string jsonData)
    {
        base.ApplyJsonData(jsonData); // Apply base class data first

        try
        {
            // Parse the JSON data to a JObject
            JObject jsonObject = JObject.Parse(jsonData);

            // Extract the party members' IDs from the JSON
            if (jsonObject.ContainsKey("party"))
            {
                JArray partyArray = (JArray)jsonObject["party"];
                if (partyArray != null)
                {
                    // Convert the array to a list of strings
                    List<string> loadedParty = partyArray.ToObject<List<string>>();

                    // Add characters to the party based on the loaded data
                    AddCharactersToParty(loadedParty);
                }
                else
                {
                    Debug.LogWarning("'party' array is null.");
                }
            }
            else
            {
                Debug.LogWarning("No 'party' key found in the JSON data.");
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"Error applying party JSON data: {ex.Message}");
        }
    }


    public void AddCharactersToParty(List<string> loadedParty)
    {

        PartyMembers = new List<CharacterObject>();

        foreach (string item in loadedParty)
        {
            if(ObjectLibrary.Library.TryGetValue(item, out SavableObject value))
            {
                PartyMembers.Add(value as CharacterObject);
            }
        }

    }


    public void SetParty(List<CharacterObject> fightParty)
    {
        PartyMembers.Clear();
        PartyMembers.AddRange(fightParty);
    }
}
