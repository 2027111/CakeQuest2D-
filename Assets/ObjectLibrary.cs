using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectLibrary : MonoBehaviour
{
    public List<CharacterObject> AllCharacterObjects = new List<CharacterObject>();
    public List<Skill> AllSkills = new List<Skill>();
    public List<InventoryItem> AllItems = new List<InventoryItem>();


    public static Dictionary<string, SavableObject> Library = new Dictionary<string, SavableObject>();
    private void Awake()
    {
        if(Library.Count == 0)
        {
            foreach (SavableObject savableObject in AllCharacterObjects)
            {
                Library.Add(savableObject.UID, savableObject);
            }

            foreach (SavableObject savableObject in AllSkills)
            {
                Library.Add(savableObject.UID, savableObject);
            }


            foreach (SavableObject savableObject in AllItems)
            {
                Library.Add(savableObject.UID, savableObject);
            }
        }
       
    }



}
