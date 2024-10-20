using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayerTransitionner : MonoBehaviour
{
    [SerializeField] int layer1;
    [SerializeField] int layer2;
    [SerializeField] int sortingLayer1;
    [SerializeField] int sortingLayer2;

    public Direction Layer1toLayer2 = Direction.Right;

    public void MoveToNextLayer()
    {
        Vector2 playerPos = Character.Player.transform.position;
        Vector2 triggerPos = transform.position;
        Vector2 difference = playerPos - triggerPos;
        Direction moveDirection = GetExitDirection(difference);

        if (moveDirection == Layer1toLayer2)
        {
            ChangeLayer(layer1, sortingLayer1, moveDirection);
        }
        else if (moveDirection == GetOppositeDirection(Layer1toLayer2))
        {
            ChangeLayer(layer2, sortingLayer2, moveDirection);
        }

    }

    private Direction GetExitDirection(Vector2 difference)
    {
        if (Layer1toLayer2 == Direction.Left || Layer1toLayer2 == Direction.Right)
        {
            return difference.x > 0 ? Direction.Right : Direction.Left;
        }
        else
        {
            return difference.y > 0 ? Direction.Top : Direction.Bottom;
        }
    }

    private void ChangeLayer(int newLayer, int newSortingLayer, Direction moveDirection)
    {
        Character.Player.transform.gameObject.layer = newLayer;
        Character.Player.Sprite.sortingLayerID = newSortingLayer;
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
}
