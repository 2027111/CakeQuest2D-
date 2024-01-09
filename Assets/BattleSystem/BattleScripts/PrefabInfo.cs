using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PrefabInfo
{
    public GameObject AttackPrefab;
    public Vector3 DefaultSpawnPosition;
    public int frame;
    public int durationInFrame;

    public bool IsFirstFrame(int frame)
    {
        return frame == this.frame;
    }
}
