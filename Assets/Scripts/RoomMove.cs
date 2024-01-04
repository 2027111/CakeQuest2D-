using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;





public enum Room1to2Orientation
{
    Left,
    Right,
    Top,
    Bottom
}



public class RoomMove : MonoBehaviour
{




    [SerializeField] RoomInfo room1;
    [SerializeField] RoomInfo room2;
    public Room1to2Orientation Room1ToRoom2 = Room1to2Orientation.Right;
    private CameraMovement camMove;




    // Start is called before the first frame update
    void Start()
    {
        camMove = Camera.main.GetComponent<CameraMovement>();
    }




    private void OnTriggerEnter2D(Collider2D other)
    {


        RoomInfo newRoom = room2 == camMove.currentRoomInfo? room1:room2;
        float vectorFactor = room2 == camMove.currentRoomInfo ? 1 : -1;
        if (other.CompareTag("Player"))
        {
            camMove.SetNewRoom(newRoom);


            other.GetComponent<Movement>().SetPosition(other.transform.position + GetPlayerChange() * vectorFactor);
            other.GetComponent<Movement>().LookAt(GetPlayerChange());

        }
    }

    public Vector3 GetPlayerChange()
    {
        Vector3 change = Vector3.zero;
        switch (Room1ToRoom2)
        {
            case Room1to2Orientation.Right:
                change = Vector2.right;
                break;
            case Room1to2Orientation.Left:
                change = Vector2.left;

                break;
            case Room1to2Orientation.Bottom:
                change = Vector2.down;

                break;
            case Room1to2Orientation.Top:
                change = Vector2.up;

                break;

            default:
                break;
        }
        return change;
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        
    }
}
