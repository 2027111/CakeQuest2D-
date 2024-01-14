using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class HitBoxInfo
{
    public Vector2 offset;
    public Vector2 size;
    public int frame;
    public bool resetCollisions;
    public int durationInFrame;
    public Vector2 knockbackVector = Vector2.right;
    public float knockbackForce = 1;
    public bool stuns;
    public float stunTime = -1;

    public bool IsFirstFrame(int frame)
    {
        return frame == this.frame;
    }
}
