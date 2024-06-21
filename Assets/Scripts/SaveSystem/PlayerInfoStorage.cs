using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerInfoStorage : MonoBehaviour
{


    public PlayerStorage infoStorage;
    [SerializeField] Character character;
    private CameraMovement camMove;


    public float minimumFadeTime = 1f;
    public static float DestroyTime = 1.4f;





    // Start is called before the first frame update
    void Start()
    {
        camMove = Camera.main.GetComponent<CameraMovement>();

        character = GetComponent<Character>();

        if (infoStorage)
        {
            if (infoStorage.forceNextChange)
            {
                MoveToScene();
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
        infoStorage.forceNextChange = true;
        FadeScreen.MoveToScene("BattleScene");

    }

    public void MoveToScene()
    {



            if (UICanvas.DialogueBoxIsActive())
            {
                UICanvas.ForceStopDialogue();
            }



        if (infoStorage)
        {

            if (infoStorage.sceneName != SceneManager.GetActiveScene().name)
            {
                infoStorage.forceNextChange = true;
                FadeScreen.MoveToScene(infoStorage.sceneName);
            }
            else
            {
                if (!FadeScreen.movingScene)
                {
                    FadeScreen.AddOnMidFadeEvent(OnTransitionOver);
                    FadeScreen.FakeMoveToScene();
                }


            }
        }
        
    }


    public void OnTransitionOver()
    {
        character.SetPosition(infoStorage.nextPosition);
        Camera.main.GetComponentInParent<CameraMovement>().ForceToTarget();
        infoStorage.forceNextChange = false;
        character.LookToward(RoomMove.DirectionToVector(infoStorage.facing));
    }

    public RoomInfo GetCurrentRoomInfo()
    {
        return infoStorage.nextRoomInfo;
    }

    public void SetNewInformationToFile()
    {
        infoStorage.sceneName = SceneManager.GetActiveScene().name;
        infoStorage.nextPosition = transform.position;
    }

    public void SetNewPosition(Vector2 newPos)
    {
        infoStorage.nextPosition = newPos;
    }

    public void SetNewRoom(RoomInfo newRoom)
    {
        if(newRoom != infoStorage.nextRoomInfo)
        {
            infoStorage.nextRoomInfo.SetValue(newRoom);
            RoomTitleCard.ShowTitle(infoStorage.nextRoomInfo.roomName);
        }
    }



    public void MoveToHomeScreenScene()
    {

        FadeScreen.MoveToScene("StartMenuScene");
    }
}
