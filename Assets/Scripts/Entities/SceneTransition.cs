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
        Resources.UnloadUnusedAssets();
        UICanvas.CancelCurrentDialogue();
        
        if (PlayerInfoStorage.InfoStorage)
        {
            PlayerInfoStorage.InfoStorage.sceneName = sceneToLoadName;
            PlayerInfoStorage.InfoStorage.nextPosition = playerPositionOnLoad;
            PlayerInfoStorage.InfoStorage.facing = facing;
        }
        FadeScreen.AddOnMidFadeEvent(OnTransitionHalf);

        FadeScreen.AddOnStartFadeEvent(Character.DeactivatePlayer);
        FadeScreen.AddOnEndFadeEvent(Character.ActivatePlayer);
        PlayerInfoStorage.CurrentInfoStorage.MoveToScene();
    }




    public void OnTransitionHalf()
    {
        Debug.Log("Stop");

        PlayerInfoStorage.CurrentInfoStorage.SetNewRoom(roomOnLoadInfo);


    }


}
