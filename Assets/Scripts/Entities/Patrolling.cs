using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Patrolling : MonoBehaviour
{


    [SerializeField] Transform[] wayPoints;
    int wayPointsIndex = 0;
    public float minimumPatrollingdistance = 0.1f;
    public float timeBetweenPoint = 0f;
    public UnityEvent OnCatchPlayer;
    public float visionRadius = 10f;
    public float visionAngle = 45f;
    public LayerMask detectionLayerMask; // LayerMask to specify which layers to detect
    [SerializeField] GameObject lightSource;


    public void SetLightSource()
    {

        if (lightSource)
        {
            Vector2 forward = GetComponent<Movement>().lookDirection;
            Vector2 position = (Vector2)lightSource.transform.position;

            // Calculate the angle in degrees that the light should be facing
            float angle = Mathf.Atan2(forward.y, forward.x) * Mathf.Rad2Deg;

            // Create a quaternion representing the rotation
            Quaternion rotation = Quaternion.Euler(0, 0, angle - 90);

            // Set the position and rotation of the light source
            lightSource.transform.SetPositionAndRotation(position, rotation);
        }
    }

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

    public bool isUsable()
    {
        return wayPoints != null && wayPoints.Length > 0 && enabled;
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
