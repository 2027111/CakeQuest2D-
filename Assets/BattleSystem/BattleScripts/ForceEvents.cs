
using System;
using UnityEngine;

[Serializable]
public class ForceEvents
{
    public Vector2 direction;
    public float force;
    public ForceMode2D forceMode;
    public int onFrame;
    public bool resetsVel;

    public bool IsFirstFrame(int frame)
    {
        return frame == this.onFrame;
    }

    public override bool Equals(object obj)
    {
        if (obj == null || GetType() != obj.GetType())
            return false;

        ForceEvents other = (ForceEvents)obj;

        return other.direction.Equals(this.direction) && other.force.Equals(this.force);
    }

    public override int GetHashCode()
    {
        unchecked
        {
            int hash = 17;
            hash = hash * 23 + direction.GetHashCode();
            hash = hash * 23 + force.GetHashCode();
            return hash;
        }
    }
}