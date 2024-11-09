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
    JSONSaveDataWrapper currentData;
    public void Start()
    {

    }


    public void SetSaveFileText()
    {
        saveText.SetText($"{currentData.saveIndex}");
        string sceneName = currentData.GetAttribute("sceneName");
        string roomName = currentData.GetAttribute("roomName");

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
