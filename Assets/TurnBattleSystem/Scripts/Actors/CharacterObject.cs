
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu]
[System.Serializable]
public class CharacterObject : SavableObject
{


    [JsonIgnore] public CharacterData characterData;
    [Space(20)]
    public int Health;
    public int MaxHealth;
    [Space(20)]
    public int Mana;
    public int MaxMana;
    [Space(20)]
    [JsonIgnore] public int Speed;
    [JsonIgnore] public int AttackDamage;
    [JsonIgnore] public float parryWindow = .15f;
    [Space(20)]
    [JsonIgnore] public List<Element> IngredientWheel = new List<Element>();

    [Space(20)]
    public bool isDead;

    [Space(20)]
    [JsonIgnore] public SkillType attackType = SkillType.Physical;
    [Space(20)]
    [JsonIgnore] public Element AttackElement;
    [Space(20)]

    [JsonIgnore] public List<GameObject> HitEffect;
    [JsonIgnore] public List<AudioClip> SoundEffect;

    [Space(40)]
    public List<Skill> Attacks;

    [Space(20)]
    [JsonIgnore] public AnimatorOverrideController animationController;

    [JsonIgnore] public BoolValue InParty;



    [Space(20)]
    [JsonIgnore] public int recipeLength = 3;


    public override string GetJsonData()
    {
        var jsonObject = JObject.Parse(base.GetJsonData()); // Start with base class data

        // Include all non-ignored properties
        jsonObject["Health"] = Health;
        jsonObject["MaxHealth"] = MaxHealth;
        jsonObject["Mana"] = Mana;
        jsonObject["MaxMana"] = MaxMana;
        jsonObject["isDead"] = isDead;

        // Handle Attacks list
        jsonObject["Attacks"] = JArray.FromObject(GetStringifiedAttackList());

        return jsonObject.ToString();



    }

    private List<string> GetStringifiedAttackList()
    {
        // Assuming each Skill has a method/property that returns its unique ID or name
        List<string> attackIds = new List<string>();
        foreach (Skill skill in Attacks)
        {
            attackIds.Add(skill.UID); // Replace with appropriate identifier for each Skill
        }
        return attackIds;
    }

    public override void ApplyData(SavableObject tempCopy)
    {
        AddSkillsToMoveset((tempCopy as CharacterObject).Attacks);

        //GameSaveManager.Singleton.StartCoroutine(AddLoadedSkillToMoveset((tempCopy as CharacterObject).Attacks));
        Health = (tempCopy as CharacterObject).Health;
        MaxHealth = (tempCopy as CharacterObject).MaxHealth;
        Mana = (tempCopy as CharacterObject).Mana;
        MaxMana = (tempCopy as CharacterObject).MaxMana;
        isDead = (tempCopy as CharacterObject).isDead;
        base.ApplyData(tempCopy);
    }


    public void AddSkillsToMoveset(List<Skill> loadedSkills)
    {

        Attacks = new List<Skill>();

        foreach (Skill item in loadedSkills)
        {
            if (ObjectLibrary.Library.TryGetValue(item.UID, out SavableObject value))
            {
                Attacks.Add(value as Skill);
            }
        }

    }

    public IEnumerator AddLoadedSkillToMoveset(List<Skill> loadedSkills)
    {
        Attacks.Clear();
        foreach (Skill skill in loadedSkills)
        {

            ResourceRequest request = Resources.LoadAsync<Skill>($"SkillFolder/{skill.name}");
            while (!request.isDone)
            {
                yield return null;
            }
            Skill loadedSkill = request.asset as Skill;
            Attacks.Add(loadedSkill);
            yield return null;
        }
        yield return null;
    }

    public void Revitalize()
    {
        isDead = false;
        Health = MaxHealth;
        Mana = MaxMana;

    }
    public GameObject GetHitEffect()
    {
        if (HitEffect.Count > 0)
        {
            return HitEffect[Random.Range(0, HitEffect.Count)];
        }
        return null;

    }

    public AudioClip GetSoundEffect()
    {
        if (SoundEffect.Count > 0)
        {
            return SoundEffect[Random.Range(0, SoundEffect.Count)];
        }
        return null;

    }

    public ElementEffect GetElementEffect(Element element)
    {
        return ElementEffect.Neutral;
    }

    public float GetParryWindowTime()
    {
        return parryWindow;
    }
}
