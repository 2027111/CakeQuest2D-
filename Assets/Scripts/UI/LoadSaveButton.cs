using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoadSaveButton : MonoBehaviour
{
    [SerializeField] Image MiddleImage;
    [SerializeField] TMP_Text saveText;
    [SerializeField] TMP_Text locationText;
    [SerializeField] TMP_Text progressText;
    [SerializeField] Image progressBar;
    [SerializeField] Image sugarCake;
    [SerializeField] Image sourCake;
    [SerializeField] Image bitterCake;
    [SerializeField] Image saltyCake;




    [SerializeField] SavableObject sugarCakeSaveObject;
    [SerializeField] SavableObject sourCakeSaveObject;
    [SerializeField] SavableObject bitterCakeSaveObject;
    [SerializeField] SavableObject saltyCakeSaveObject;


    [SerializeField] SavableObject PlayerStorageSaveObject;

    JSONSaveDataWrapper currentData;


    public void Start()
    {
        // Example use of SetCake in Start to set initial states
        SetCakes();
    }

    public void SetCakes()
    {
        // Set all cake images to pure black
        SetCake(sugarCake, currentData.GetAttributeOfIdObject(sugarCakeSaveObject.UID, "RuntimeValue"));
        SetCake(sourCake, currentData.GetAttributeOfIdObject(sourCakeSaveObject.UID, "RuntimeValue"));
        SetCake(bitterCake, currentData.GetAttributeOfIdObject(bitterCakeSaveObject.UID, "RuntimeValue"));
        SetCake(saltyCake, currentData.GetAttributeOfIdObject(saltyCakeSaveObject.UID, "RuntimeValue"));
    }

    public void SetCake(Image cake, string unlockedText)
    {
        // Set the image color based on the unlocked state

        bool unlocked = false;


        if (Boolean.TryParse(unlockedText, out bool value))
        {
            unlocked = value;
        }
        if (cake != null)
            cake.color = unlocked ? Color.white : Color.black;
    }

    public float GetProgress()
    {
        int amountOfBoolValue = 0;
        int amountOfTrueValue = 0;

        foreach (string saveObject in currentData.Data)
        {
            try
            {
                // Parse the string into a JObject
                JObject jsonObject = JObject.Parse(saveObject);

                // Check if it contains the "RunTimeValue" attribute
                if (jsonObject.ContainsKey("RuntimeValue"))
                {
                    amountOfBoolValue++;
                    JToken value = jsonObject["RuntimeValue"];

                    // Check if it's a boolean and increment counters
                    if (value.Type == JTokenType.Boolean)
                    {
                        if ((bool)value) amountOfTrueValue++;
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle or log parsing errors if saveObject is not valid JSON
                Console.WriteLine($"Invalid JSON: {saveObject} - {ex.Message}");
            }
        }

        // Calculate and return progress as a percentage
        return (amountOfBoolValue > 0 ? (float)amountOfTrueValue / amountOfBoolValue : 0f) * 100;
    }


    public void SetSaveFileText()
    {
        saveText.SetText($"{currentData.saveIndex}");
        string sceneName = currentData.GetAttribute("sceneName");
        string roomName = currentData.GetAttribute("roomName");
        int percentage = (int)GetProgress();
        progressBar.fillAmount = percentage/100f;
        progressText.text = percentage + "%";
        locationText.SetText(sceneName + " | " + roomName);
    }

    public void OnSelect()
    {
        SFXPlayer.instance.PlayOnNavigate();
    }

    public void OnClick()
    {
        SFXPlayer.instance.PlayOnSelect();
    }
    public void SetSaveFile(JSONSaveDataWrapper saveData)
    {
        currentData = saveData;
        SetSaveFileText();
    }
}
