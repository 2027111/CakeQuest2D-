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
        SetNewRoom(currentRoomInfo);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 targetPos = new Vector3(target.position.x, target.position.y, transform.position.z);
        targetPos.x = Mathf.Clamp(targetPos.x, currentRoomInfo.minCameraOffset.x, currentRoomInfo.maxCameraOffset.x);
        targetPos.y = Mathf.Clamp(targetPos.y, currentRoomInfo.minCameraOffset.y, currentRoomInfo.maxCameraOffset.y);
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
        Vector3 targetPos = new Vector3(target.position.x, target.position.y, transform.position.z);

        targetPos.x = Mathf.Clamp(targetPos.x, currentRoomInfo.minCameraOffset.x, currentRoomInfo.maxCameraOffset.x);
        targetPos.y = Mathf.Clamp(targetPos.y, currentRoomInfo.minCameraOffset.y, currentRoomInfo.maxCameraOffset.y);

        transform.position = targetPos;
    }

    public void SetNewRoom(RoomInfo newRoom)
    {
        currentRoomInfo = newRoom;
        if (currentRoomInfo)
        {
            RoomTitleCard.ShowTitle(currentRoomInfo.roomName);
        }
    }
}
