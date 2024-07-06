#if UNITY_EDITOR
using System;
using UnityEditor;
#endif
using UnityEngine;

public class SavableObject : ScriptableObject
{
    public string UID;

    private void OnValidate()
    {
#if UNITY_EDITOR
        if (string.IsNullOrEmpty(UID))
        {
            UID = GUID.Generate().ToString();
            EditorUtility.SetDirty(this);
        }
#endif
    }

    public virtual void ApplyData(SavableObject tempCopy)
    {
    }

    public bool Matches(SavableObject obj)
    {
        if (GetType().Name == obj.GetType().Name)
        {
            if (UID == obj.UID)
            {
                return true;
            }
        }
        return false;
    }

}