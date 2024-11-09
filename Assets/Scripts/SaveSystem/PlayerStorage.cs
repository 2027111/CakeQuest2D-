
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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



    public override string GetJsonData()
    {

        var jsonObject = JObject.Parse(base.GetJsonData()); // Start with base class data


        jsonObject["sceneName"] = sceneName; // Adding additional data
        jsonObject["facing"] = facing.ToString(); // Adding additional data
        jsonObject["forceNextChange"] = forceNextChange; // Adding additional data

        jsonObject["nextPos"] = new JObject
        {
            ["x"] = nextPosition.x,
            ["y"] = nextPosition.y
        };

     
        return jsonObject.ToString();



    }



    public override void ApplyJsonData(string jsonData)
    {
        base.ApplyJsonData(jsonData); // Apply base class data first

        // Parse the JSON data to a JObject
        JObject jsonObject = JObject.Parse(jsonData);


        // Apply minCameraOffset from JSON to the property
        if (jsonObject["nextPos"] != null)
        {
            nextPosition = new Vector2(
                jsonObject["nextPos"]["x"]?.Value<float>() ?? nextPosition.x,
                jsonObject["nextPos"]["y"]?.Value<float>() ?? nextPosition.y
            );
        }

        if (jsonObject["facing"] != null)
        {
            string facingStr = jsonObject["facing"].ToString();
            if (Enum.TryParse(facingStr, out Direction direction))
            {
                facing = direction; // Apply direction
            }
            else
            {
                Debug.LogError($"Invalid direction string: {facingStr}");
            }
        }
    }





}
