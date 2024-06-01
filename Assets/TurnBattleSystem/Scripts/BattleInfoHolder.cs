using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class BattleInfoHolder : SavableObject
{
    public BattleInfo battleInfo;

    public void SetInfos(BattleInfo thisbattleInfo)
    {
        battleInfo = thisbattleInfo;
    }
}
