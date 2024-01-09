using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TeamIndex
{
    None = -1,
    Neutral = 0,
    Player,
    Enemy,
    Count
}
public class TeamComponent : MonoBehaviour
{
    [SerializeField] private TeamIndex _teamIndex = TeamIndex.None;


    public TeamIndex teamIndex
    {
        set
        {
            if(_teamIndex != value)
            {
                _teamIndex = value;
            }
        }
        get
        {
            return _teamIndex;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
