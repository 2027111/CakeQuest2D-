using System.Collections;
using System.Collections.Generic;
using UnityEngine;




[System.Serializable]
public class Condition
{
    public BoolValue condition;
    public GameObject prefab;
    public Vector3 position;
}
public class ConditionSpawner : MonoBehaviour
{
    public List<Condition> conditions = new List<Condition>();
}
