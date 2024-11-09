using Newtonsoft.Json.Linq;
using System;
using UnityEngine;

[CreateAssetMenu(fileName = "RoomInfo", menuName = "ScriptableObjects/RoomInfo", order = 1)]
[Serializable]
public class RoomInfo : SavableObject
{
    public string roomName;
    public Vector2 minCameraOffset;
    public Vector2 maxCameraOffset;

    public override void ApplyData(SavableObject tempCopy)
    {

        minCameraOffset = (tempCopy as RoomInfo).minCameraOffset;
        maxCameraOffset = (tempCopy as RoomInfo).maxCameraOffset;
        base.ApplyData(tempCopy);
    }



    public override string GetJsonData()
    {

        var jsonObject = JObject.Parse(base.GetJsonData()); // Start with base class data

        jsonObject["roomName"] = roomName;

        // Convert Vector2 properties to JSON format
        jsonObject["minCameraOffset"] = new JObject
        {
            ["x"] = minCameraOffset.x,
            ["y"] = minCameraOffset.y
        };

        jsonObject["maxCameraOffset"] = new JObject
        {
            ["x"] = maxCameraOffset.x,
            ["y"] = maxCameraOffset.y
        };

        return jsonObject.ToString();



    }


    public static bool operator ==(RoomInfo c1, RoomInfo c2)
    {
        if (ReferenceEquals(c1, c2))
            return true;

        if (ReferenceEquals(c1, null) || ReferenceEquals(c2, null))
            return false;

        return c1.minCameraOffset == c2.minCameraOffset && c1.maxCameraOffset == c2.maxCameraOffset;
    }

    public static bool operator !=(RoomInfo c1, RoomInfo c2)
    {
        return !(c1 == c2);
    }

    public override bool Equals(object obj)
    {
        if (obj == null || GetType() != obj.GetType())
            return false;

        RoomInfo other = (RoomInfo)obj;
        return this.minCameraOffset == other.minCameraOffset && this.maxCameraOffset == other.maxCameraOffset;
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }

    public void SetValue(RoomInfo newRoom)
    {
        roomName = newRoom.roomName;
        minCameraOffset = newRoom.minCameraOffset;
        maxCameraOffset = newRoom.maxCameraOffset;
        RoomTitleCard.ShowTitle(roomName);
    }
}
