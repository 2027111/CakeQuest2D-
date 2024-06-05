using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public class ItemsToJsonConverter: MonoBehaviour
{
    public List<InventoryItem> items; // Assign skills in the Inspector

    [ContextMenu("Convert Items to JSON")]
    public void ConvertItemsToJson()
    {
        string jsonOutput = "";

        foreach (InventoryItem item in items)
        {
            // Create the inner JSON object for skill data
            var innerJsonData = new Dictionary<string, string>
            {
                { "itemName", item.itemId },
                { "itemDescription", item.itemDescription }
            };

            // Serialize the inner JSON object to a string
            string innerJsonString = JsonConvert.SerializeObject(innerJsonData, Formatting.None);

            // Create the outer JSON object with dataId and inner JSON string
            var outerJsonData = new Dictionary<string, object>
            {
                { "dataId", "item_" + item.itemId },
                { "jsonData", innerJsonString }
            };

            // Serialize the outer JSON object to a string
            string outerJsonString = JsonConvert.SerializeObject(outerJsonData, Formatting.None);

            // Add the serialized outer JSON string to the final output string
            jsonOutput += outerJsonString + ",";
        }

        // Remove the last comma
        if (jsonOutput.EndsWith(","))
        {
            jsonOutput = jsonOutput.Substring(0, jsonOutput.Length - 1);
        }

        // Log the final JSON output
        Debug.Log(jsonOutput);
        Debug.Log(Application.dataPath);

        // Optional: Write JSON to a file
        System.IO.File.WriteAllText(Application.dataPath + "/Items.json", jsonOutput);
    }
}
