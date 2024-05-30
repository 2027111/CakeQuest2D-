using System.Collections;
using System.Collections.Generic;
using UnityEngine;






[CreateAssetMenu]
public class Party : SavableObject
{

    public List<CharacterObject> PartyMembers = new List<CharacterObject>();

    public override void ApplyData(SavableObject tempCopy)
    {
        GameSaveManager.Singleton.StartCoroutine(AddLoadedCharactersToMoveset((tempCopy as Party).PartyMembers));
        base.ApplyData(tempCopy);
    }

    public IEnumerator AddLoadedCharactersToMoveset(List<CharacterObject> loadedParty)
    {
        PartyMembers.Clear();


        foreach (CharacterObject item in loadedParty)
        {
            Debug.Log(item.name);
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
