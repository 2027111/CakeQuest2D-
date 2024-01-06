using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;





public enum Direction
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
    public Direction Room1ToRoom2 = Direction.Right;
    private CameraMovement camMove;
    Character player;




    // Start is called before the first frame update
    void Start()
    {
        camMove = Camera.main.GetComponent<CameraMovement>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Character>();

    }


    public void MoveToNextRoom()
    {
        RoomInfo newRoom = room2 == camMove.currentRoomInfo ? room1 : room2;

        float vectorFactor = room2 == camMove.currentRoomInfo ? 1 : -1;
        Vector2 newPos = player.transform.position + DirectionToVector(Room1ToRoom2) * vectorFactor;
        camMove.SetNewRoom(newRoom);

        player.GetComponent<PlayerInfoStorage>().SetNewRoom(newRoom);
        player.GetComponent<PlayerInfoStorage>().SetNewPosition(newPos);
        player.GetComponent<Movement>().SetPosition(newPos);
        player.GetComponent<Movement>().LookAt(DirectionToVector(Room1ToRoom2));

        
    }


    public static Vector3 DirectionToVector(Direction dir)
    {
        Vector3 change = Vector3.zero;
        switch (dir)
        {
            case Direction.Right:
                change = Vector2.right;
                break;
            case Direction.Left:
                change = Vector2.left;

                break;
            case Direction.Bottom:
                change = Vector2.down;

                break;
            case Direction.Top:
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
