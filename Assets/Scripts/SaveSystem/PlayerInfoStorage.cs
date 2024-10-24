using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerInfoStorage : MonoBehaviour
{


    public PlayerStorage infoStorage;
    public static PlayerStorage InfoStorage;
    public static PlayerInfoStorage CurrentInfoStorage;
    [SerializeField] Character character;
    private CameraMovement camMove;


    public float minimumFadeTime = 1f;
    public static float DestroyTime = 1.4f;

    private void Awake()
    {
        InfoStorage = infoStorage;
        CurrentInfoStorage = this;
    }



    // Start is called before the first frame update
    void Start()
    {
        camMove = Camera.main.GetComponent<CameraMovement>();

        character = GetComponent<Character>();

        if (InfoStorage)
        {
            if (InfoStorage.forceNextChange)
            {
                OnTransitionOver();
            }
        }
        else
        {
            if (!FadeScreen.movingScene)
            {
                FadeScreen.StartTransition(false, Color.black, .5f);
            }
        }
    }


    public void GoToBattleScene()
    {
        SetNewInformationToFile();
        InfoStorage.forceNextChange = true;
        FadeScreen.MoveToScene("BattleScene");

    }

    public void MoveToScene()
    {



        if (UICanvas.DialogueBoxIsActive())
        {
            UICanvas.ForceStopDialogue();
        }



        if (InfoStorage)
        {

            if (InfoStorage.sceneName != SceneManager.GetActiveScene().name)
            {
                InfoStorage.forceNextChange = true;
                FadeScreen.MoveToScene(InfoStorage.sceneName);
            }
            else
            {
                if (!FadeScreen.movingScene)
                {
                    FadeScreen.FakeMoveToScene();
                    FadeScreen.AddOnMidFadeEvent(OnTransitionOver);
                }


            }
        }

    }


    public void OnTransitionOver()
    {
        //Debug.Log("Forced Pos : " + InfoStorage.nextPosition);
        character.SetPosition(InfoStorage.nextPosition);
        Camera.main.GetComponentInParent<CameraMovement>().ForceToTarget();
        InfoStorage.forceNextChange = false;
        character.LookToward(RoomMove.DirectionToVector(InfoStorage.facing));
    }

    public RoomInfo GetCurrentRoomInfo()
    {
        return InfoStorage.nextRoomInfo;
    }

    public void SetNewInformationToFile()
    {
        InfoStorage.sceneName = SceneManager.GetActiveScene().name;
        InfoStorage.nextPosition = transform.position;
    }

    public void SetNewPosition(Vector2 newPos)
    {
        InfoStorage.nextPosition = newPos;
    }

    public void SetNewRoom(RoomInfo newRoom)
    {
        if (newRoom != InfoStorage.nextRoomInfo)
        {
            InfoStorage.nextRoomInfo.SetValue(newRoom);
            RoomTitleCard.ShowTitle(InfoStorage.nextRoomInfo.roomName);
        }
    }



    public void MoveToHomeScreenScene()
    {
        FadeScreen.MoveToScene("StartMenuScene");
    }
}
