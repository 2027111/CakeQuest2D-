using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleStarter : MonoBehaviour
{


    public BattleInfo ThisbattleInfo;
    public BattleInfoHolder currentBattleInfo;



    public  void StartBattle()
    {
        currentBattleInfo.SetInfos(ThisbattleInfo);
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInfoStorage>().GoToBattleScene();
    }
}