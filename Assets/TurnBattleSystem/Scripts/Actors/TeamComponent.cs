using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TeamIndex
{
    Player = 1,
    Enemy,
}
public class TeamComponent : MonoBehaviour
{
    [SerializeField] private TeamIndex _teamIndex = TeamIndex.Player;


    public TeamIndex teamIndex
    {
        set
        {
            if (_teamIndex != value)
            {
                _teamIndex = value;
            }
        }
        get
        {
            return _teamIndex;
        }
    }






    public static Color TeamColor(TeamIndex index)
    {
        switch (index)
        {
            case TeamIndex.Player:
                return Color.cyan;
            case TeamIndex.Enemy:
                return Color.red;
        }

        return Color.white;
    }
}
