using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu]
public class Party : SavableObject
{

    public List<CharacterObject> PartyMembers = new List<CharacterObject>();
    public UnityEvent OnAddToParty;
    public override void ApplyData(SavableObject tempCopy)
    {
        GameSaveManager.Singleton.StartCoroutine(AddLoadedCharactersToParty((tempCopy as Party).PartyMembers));
        base.ApplyData(tempCopy);
    }



    public IEnumerator AddLoadedCharactersToParty(List<CharacterObject> loadedParty)
    {
        PartyMembers.Clear();


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


}
