using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{



    public Transform target;
    public float smoothing;

    [SerializeField] public RoomInfo currentRoomInfo;




    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        currentRoomInfo = PlayerInfoStorage.InfoStorage.nextRoomInfo;
        Vector3 targetPos = GetCamPosition();
        transform.position = targetPos;

    }


    public Vector3 GetCamPosition()
    {
        Vector3 targetPos = new Vector3(target.position.x, target.position.y, transform.position.z);
        if (currentRoomInfo)
        {

            targetPos.x = Mathf.Clamp(targetPos.x, currentRoomInfo.minCameraOffset.x, currentRoomInfo.maxCameraOffset.x);
            targetPos.y = Mathf.Clamp(targetPos.y, currentRoomInfo.minCameraOffset.y, currentRoomInfo.maxCameraOffset.y);
        }
        return targetPos;
    }
    public void StartShaking(bool shaking)
    {

    }
    // Update is called once per frame
    void FixedUpdate()
    {
        SetPosition();
    }

    public void SetPosition()
    {
        Vector3 targetPos = GetCamPosition();
        if (transform.position != targetPos)
        {
            transform.position = Vector3.Lerp(transform.position, targetPos, smoothing);
        }
    }

    public RoomInfo GetCurrentRoom()
    {
        return currentRoomInfo;
    }

    public void ForceToTarget()
    {
        if (target)
        {

            Vector3 targetPos = new Vector3(target.position.x, target.position.y, transform.position.z);
            targetPos.x = Mathf.Clamp(targetPos.x, currentRoomInfo.minCameraOffset.x, currentRoomInfo.maxCameraOffset.x);
            targetPos.y = Mathf.Clamp(targetPos.y, currentRoomInfo.minCameraOffset.y, currentRoomInfo.maxCameraOffset.y);
            transform.position = targetPos;
        }
    }


}
