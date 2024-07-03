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


    public IEnumerator ClearRessources()
    {

        AsyncOperation op = Resources.UnloadUnusedAssets();
        while (!op.isDone)
        {
            yield return null;
        }
    }



    public void OnTransitionHalf()
    {

        PlayerInfoStorage.CurrentInfoStorage.SetNewRoom(roomOnLoadInfo);
        ClearRessources();


    }


}
