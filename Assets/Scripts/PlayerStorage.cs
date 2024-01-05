
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerStorageObject", menuName = "ScriptableObjects/PlayerStorageObject", order = 1)]
public class PlayerStorage : ScriptableObject
{

    public string sceneName = "Main";
    public RoomInfo nextRoomInfo = null;
    public Vector2 nextPosition = Vector2.zero;
    public Direction facing;
    public bool forceNextChange = false;

}
