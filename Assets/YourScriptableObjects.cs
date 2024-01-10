using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class YourScriptableObject : ScriptableObject
{
    [SerializeField]
    private string persistentId; // Serialized field to store the persistent identifier

    public Guid Id { get; set; }

    // Rest of your ScriptableObject properties and methods

    private void OnEnable()
    {
        if (string.IsNullOrEmpty(persistentId))
        {
            // If persistentId is not set, generate a new Guid and save it
            Id = Guid.NewGuid();
            persistentId = Id.ToString();
#if UNITY_EDITOR
            UnityEditor.EditorUtility.SetDirty(this);
#endif
        }
        else
        {
            // If persistentId is set, parse it to Guid
            Id = Guid.Parse(persistentId);
        }
    }
}
