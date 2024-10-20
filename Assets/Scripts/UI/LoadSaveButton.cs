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
    ReadableSaveData currentData;
    public void Start()
    {

    }


    public void SetSaveFileText()
    {
        saveText.SetText($"{currentData.saveIndex}");
        PlayerStorage pis = ((PlayerStorage)currentData.data[0]);
        RoomInfo currentRoom = ((RoomInfo)currentData.data[1]);

        locationText.SetText(pis.sceneName + " | " + currentRoom.roomName);
    }

    public void OnSelect()
    {
        SFXPlayer.instance.PlayOnNavigate();
    }

    public void OnClick()
    {
        SFXPlayer.instance.PlayOnSelect();
    }
    public void SetSaveFile(ReadableSaveData saveData)
    {
        currentData = saveData;
        SetSaveFileText();
    }
}
