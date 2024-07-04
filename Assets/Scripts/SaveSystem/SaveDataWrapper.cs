using System;
using System.Collections.Generic;



[Serializable]
public class SaveDataWrapper
{
    public List<CharacterInventory> AllInventories { get; set; }
    public List<CharacterObject> AllCharacterObjects { get; set; }
    public List<Party> AllParties { get; set; }

}