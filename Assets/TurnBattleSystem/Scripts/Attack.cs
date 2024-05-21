using System;
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
[CreateAssetMenu]
public class Attack : ScriptableObject
{




    public TargetType targetType = TargetType.Single;
    public Friendliness friendliness = Friendliness.Non_Friendly;
    public TargetStateType targetStateType = TargetStateType.Alive;
    public SkillType skillType = SkillType.Physical;
    public string Name;
    public AnimationClip animationClip;
    public GameObject HitEffect;
    public int baseDamage = -10; //Negative for damage, positive for healing
    [Range(50, 100)]
    public int baseAccuracy = 80; //Negative for damage, positive for healing
    public int manaCost = 14;
    public string[] spawnObjects;
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
        Command c = new OffensiveSkillCommand(this);

        switch (friendliness)
        {
            case Friendliness.Friendly:
                c = new DefensiveSkillCommand(this);
                break;

            case Friendliness.Non_Friendly:

                c = new OffensiveSkillCommand(this);
                break;

            case Friendliness.Neutral:

                c = new OffensiveSkillCommand(this);
                break;
        }

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
}
