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

    public void ConfirmBattle()
    {
        if (battleInfo)
        {
            battleInfo.SetRuntime();
            if (battleInfo.CutsceneForDialogue != null)
            {

                battleInfo.CutsceneForDialogue.ForceRuntime();

            }
        }
    }
}
