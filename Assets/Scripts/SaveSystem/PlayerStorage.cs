
using Newtonsoft.Json;
using System;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerStorageObject", menuName = "ScriptableObjects/PlayerStorageObject", order = 1)]
[System.Serializable]
public class PlayerStorage : SavableObject
{
    [Header("Runtime")]
    public string sceneName = "Main";
    [JsonIgnore] public RoomInfo nextRoomInfo = null;
    public Vector2 nextPosition = Vector2.zero;
    public Direction facing;
    public bool forceNextChange = false;

    public override void ApplyData(SavableObject tempCopy)
    {

        sceneName = ((tempCopy as PlayerStorage).sceneName);
        nextPosition = ((tempCopy as PlayerStorage).nextPosition);
        facing = ((tempCopy as PlayerStorage).facing);
        base.ApplyData(tempCopy);
    }


}
