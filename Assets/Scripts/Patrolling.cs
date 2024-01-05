using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patrolling : MonoBehaviour
{


    [SerializeField] Transform[] wayPoints;
    int wayPointsIndex = 0;
    public float minimumPatrollingdistance = 0.1f;


    private void Start()
    {
        if (wayPoints.Length < 2)
        {
            Destroy(this);
        }
    }

    public Transform NextWayPoint()
    {

        wayPointsIndex++;
        if (wayPointsIndex >= wayPoints.Length)
        {
            wayPointsIndex = 0;
        }



        return wayPoints[wayPointsIndex];
    }


    public Transform GetStartingWayPoint()
    {
        return wayPoints[0];
    }

    public Transform GetCurrentWayPoint()
    {
        return wayPoints[wayPointsIndex];
    }

}
