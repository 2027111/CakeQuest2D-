

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Newtonsoft.Json;

public enum TargetType
{
    Single,
    Full
}
public enum SkillType
{
    Physical,
    Magic,
    Ranged
}
public enum Friendliness
{
    Friendly,
    Non_Friendly,
    Neutral
}

public enum TargetStateType
{
    Alive,
    Dead
}



[CreateAssetMenu(fileName = "Basic Skill", menuName = "Skills/BaseSkill")]
public class Skill : IActionData
{


    [JsonIgnore] public SkillType skillType = SkillType.Physical;
    public string skillId;
    [JsonIgnore] public string Description = "Attack qui fait mal :)";
    [JsonIgnore] public AnimationClip animationClip;
    [JsonIgnore] public string voiceClipId;
    [JsonIgnore] public int baseDamage = -10; //Negative for damage, positive for healing
    [Range(50, 100)]
    [JsonIgnore] public int baseAccuracy = 80; //Negative for damage, positive for healing
    [JsonIgnore] public int manaCost = 14;
    [JsonIgnore] public string[] spawnObjects;

    
    public string GetName()
    {
        string name = skillId;
        string newName = LanguageData.GetDataById("skill_" + skillId).GetValueByKey("skillName");
        if (newName != "E404")
        {
            return newName;
        }
        return name;
    }

    public string GetDescription()
    {
        string desc = Description;
        string newDesc = LanguageData.GetDataById("skill_" + skillId).GetValueByKey("skillDescription");
        if(newDesc != "E404")
        {
            newDesc = NewDialogueStarterObject.GetFormattedLines(this, newDesc);
            return newDesc;
        }
        return desc;
    }


    public string GetElement()
    {
        string elem = element.ToString();
        string newElem = LanguageData.GetDataById("element").GetValueByKey(elem);
        if (newElem != "E404")
        {
            return newElem;
        }
        return newElem;
    }


    public int turnTilTriggered = 0;
    public int turnTilActive = 0;


    public override string ToString()
    {
        return skillId;
    }


    public Command GetCommandType()
    {
        Command c = new SkillCommand(this);
        return c;
    }


    public string GetSpawnObject(int i)
    {
        if (i < spawnObjects.Length)
        {
            return spawnObjects[i];
        }

        return null;
    }



    public virtual void UseSkill(BattleCharacter Source, List<BattleCharacter> Target)
    {
        foreach (BattleCharacter target in Target)
        {
            if (target.Entity.isDead == false)
            {

                //float sourceSpeed = Source.Speed * (attack.baseAccuracy / 100f);
                //float targetSpeed = target.Speed;
                //float adjustedSpeed = sourceSpeed - targetSpeed * 0.5f; // Weigh target speed less to favor source speed

                //float dodgeThreshold = adjustedSpeed + targetSpeed;
                //float dodge = Random.Range(0f, dodgeThreshold);
                CharacterObject characterObject = target.GetReference();

                ElementEffect elementEffect = characterObject.GetElementEffect(element);




                if (elementEffect == ElementEffect.NonAffected)
                {
                    target.GetComponent<TextEffect>().SpawnTextEffect(LanguageData.GetDataById("Indications").GetValueByKey("null"), Color.white, target.GetComponent<TextEffect>().GetAspectTextPosition());
                }
                else
                {

                    AttackInformation info = new AttackInformation(this, elementEffect, Source);
                    target?.Entity.AddToHealth(info);
                }
            }

        }
    }

    public override int GetAmount()
    {
        return baseDamage;
    }
}
