using System;
using System.Collections.Generic;



[Serializable]
public class SaveDataWrapper
{
    public List<CharacterInventory> AllInventories = new List<CharacterInventory>();
    public List<CharacterObject> AllCharacterObjects = new List<CharacterObject>();
    public List<Party> AllParties = new List<Party>();
    public List<SavableObject> Data = new List<SavableObject>();
    public float playTime = 0;
}