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

    public bool IsFirstFrame(int frame)
    {
        return frame == this.frame;
    }
}
