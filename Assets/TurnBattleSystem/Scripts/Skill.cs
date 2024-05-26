
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
public class Skill : ScriptableObject
{

    public TargetType targetType = TargetType.Single;
    public Friendliness friendliness = Friendliness.Non_Friendly;
    public TargetStateType targetStateType = TargetStateType.Alive;
    public SkillType skillType = SkillType.Physical;
    public Element element = Element.Slash;
    public string Name;
    public string Description = "Attack qui fait mal :)";
    public AnimationClip animationClip;
    public List<GameObject> HitEffect;
    public List<AudioClip> SoundEffect;
    public string voiceClipId;
    public int baseDamage = -10; //Negative for damage, positive for healing
    [Range(50, 100)]
    public int baseAccuracy = 80; //Negative for damage, positive for healing
    public int manaCost = 14;
    public string[] spawnObjects;





    public int turnTilTriggered = 0;
    public int turnTilActive = 0;
    public bool GetFriendly()
    {
        return friendliness == Friendliness.Friendly;
    }


    public override string ToString()
    {
        return Name;
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
                    target.GetComponent<TextEffect>().SpawnTextEffect("Nullified", Color.white, target.GetComponent<TextEffect>().GetAspectTextPosition());
                }
                else
                {
                    target?.Entity.AddToHealth(this, elementEffect, Source);
                }
            }

        }
    }
}
