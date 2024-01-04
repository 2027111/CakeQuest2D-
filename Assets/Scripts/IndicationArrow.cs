using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndicationArrow : MonoBehaviour
{
    public GameObject targetObject;
    [SerializeField] Transform arrow;

    void Update()
    {
        if (targetObject != null)
        {
            // Get the direction from the current position to the target object
            Vector3 directionToTarget = targetObject.transform.position - arrow.position;

            // Set the rotation to look at the target object
            arrow.rotation = Quaternion.LookRotation(Vector3.forward, directionToTarget.normalized);
        }
    }

    public void SetObject(GameObject p)
    {
        if(p == null)
        {
            targetObject = null;
            arrow.gameObject.SetActive(false);
        }
        else
        {
            targetObject = p;
            arrow.gameObject.SetActive(true);
        }
    }
}
