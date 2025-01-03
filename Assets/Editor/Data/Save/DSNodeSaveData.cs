using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[Serializable]
public class DSNodeSaveData
{
    [field: SerializeField] public List<ConditionResultObject> Conditions { get; set; }
    [field: SerializeField] public List<BattleCondition> BattleConditionParams { get; set; }
    [field: SerializeField] public string ID { get; set; }
    [field: SerializeField] public string Name { get; set; }
    [field: SerializeField] public string EventIndex { get; set; }
    [field: SerializeField] public List<string> Text { get; set; }
    [field: SerializeField] public List<DSChoiceSaveData> Choices { get; set; }
    [field: SerializeField] public string GroupID { get; set; }
    [field: SerializeField] public DSDialogueType DialogueType { get; set; }
    [field: SerializeField] public Vector2 Position { get; set; }
    
}
