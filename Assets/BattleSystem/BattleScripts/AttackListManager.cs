using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum AttackPlacement
{
    NONE,
    NLIGHT,
    SLIGHT,
    DLIGHT,
    NSPECIAL,
    SSPECIAL,
    DSPECIAL,
}
public class AttackListManager : MonoBehaviour
{
    public List<AttackData> AttackList = new List<AttackData>();


    public AttackData GetAttackMatching(AttackPlacement placement)
    {
        if(AttackList.Count < 0)
        {
            return null;
        }
        foreach(AttackData data in AttackList)
        {
            if(data.attackPlacement == placement)
            {
                return data;
            }
        }
        return null;
    }

    public void SetAttackList(List<AttackData> attackList)
    {
        if (attackList.Count > 0)
        {

            AttackList = attackList;
        }
    }
}
