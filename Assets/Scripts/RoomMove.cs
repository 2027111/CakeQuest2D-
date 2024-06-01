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
    Character player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Character>();
    }

    public void MoveToNextRoom()
    {
            Vector2 playerPos = player.transform.position;
            Vector2 triggerPos = transform.position;
            Vector2 difference = playerPos - triggerPos;
            Direction moveDirection = GetExitDirection(difference);

            if (moveDirection == Room1ToRoom2)
            {
                MovePlayer(room1, moveDirection);
            }
            else if (moveDirection == GetOppositeDirection(Room1ToRoom2))
            {
                MovePlayer(room2, moveDirection);
            }
       
    }

    private Direction GetExitDirection(Vector2 difference)
    {
        if (Room1ToRoom2 == Direction.Left || Room1ToRoom2 == Direction.Right)
        {
            return difference.x > 0 ? Direction.Right : Direction.Left;
        }
        else
        {
            return difference.y > 0 ? Direction.Top : Direction.Bottom;
        }
    }

    private void MovePlayer(RoomInfo newRoom, Direction moveDirection)
    {
        Vector2 newPos = (Vector2)player.transform.position;// + (Vector2)DirectionToVector(moveDirection);
        player.GetComponent<PlayerInfoStorage>().SetNewRoom(newRoom);
        player.GetComponent<PlayerInfoStorage>().SetNewPosition(newPos);
        if (player.GetComponent<Character>().GetState() != "NothingBehaviour")
        {
            //player.GetComponent<Movement>().SetPosition(newPos);
            player.GetComponent<Movement>().LookAt(DirectionToVector(moveDirection));
        }
    }

    private Direction GetOppositeDirection(Direction direction)
    {
        switch (direction)
        {
            case Direction.Right: return Direction.Left;
            case Direction.Left: return Direction.Right;
            case Direction.Top: return Direction.Bottom;
            case Direction.Bottom: return Direction.Top;
            default: return Direction.Right;
        }
    }

    public static Vector3 DirectionToVector(Direction dir)
    {
        Vector3 change = Vector3.zero;
        switch (dir)
        {
            case Direction.Right: change = Vector2.right; break;
            case Direction.Left: change = Vector2.left; break;
            case Direction.Bottom: change = Vector2.down; break;
            case Direction.Top: change = Vector2.up; break;
            default: break;
        }
        return change;
    }

    public RoomInfo GetCurrentRoom()
    {
        return player.GetComponent<PlayerInfoStorage>().GetCurrentRoomInfo();
    }
}
