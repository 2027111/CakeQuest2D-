
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerStorageObject", menuName = "ScriptableObjects/PlayerStorageObject", order = 1)]
public class PlayerStorage : ScriptableObject
{
    public RoomInfo nextRoomInfo = null;
    public Vector2 nextPosition = Vector2.zero;
    public bool forceNewInfo = false;

}
