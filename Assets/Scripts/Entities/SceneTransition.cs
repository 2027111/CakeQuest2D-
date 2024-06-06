using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{


    public string sceneToLoadName;
    public Vector2 playerPositionOnLoad;
    public RoomInfo roomOnLoadInfo;
    public Direction facing;



    public void TransitionScene()
    {
        UICanvas.CancelCurrentDialogue();
        PlayerInfoStorage storage = Character.Player.GetComponent<PlayerInfoStorage>();
        if (storage)
        {
            storage.infoStorage.sceneName = sceneToLoadName;
            storage.infoStorage.nextPosition = playerPositionOnLoad;
            storage.infoStorage.facing = facing;
        }
        FadeScreen.AddOnMidFadeEvent(OnTransitionHalf);

        FadeScreen.AddOnStartFadeEvent(Character.DeactivatePlayer);
        FadeScreen.AddOnEndFadeEvent(Character.ActivatePlayer);
        storage.MoveToScene();
        //SceneManager.LoadScene(sceneToLoadName);
    }




    public void OnTransitionHalf()
    {
        Debug.Log("Stop");
        PlayerInfoStorage storage = Character.Player.GetComponent<PlayerInfoStorage>();
        storage.infoStorage.nextRoomInfo.SetValue(roomOnLoadInfo);


    }


}
