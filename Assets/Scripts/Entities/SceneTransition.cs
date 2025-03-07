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


    public bool CustomTransition = false;
    public float transitionFadeTime = .2f;
    public float transitionTime = 1f;
    public Color transitionColor;



    public void TransitionScene()
    {
        UICanvas.CancelCurrentDialogue();

        if (PlayerInfoStorage.InfoStorage)
        {
            PlayerInfoStorage.InfoStorage.sceneName = sceneToLoadName;
            PlayerInfoStorage.InfoStorage.nextPosition = playerPositionOnLoad;
            PlayerInfoStorage.InfoStorage.facing = facing;
        }
        if (CustomTransition)
        {
            FadeScreen.SetColor(transitionColor);
            FadeScreen.SetTimes(transitionFadeTime, transitionTime);
        }
        else
        {
            FadeScreen.ResetTimes();
        }
        FadeScreen.AddOnMidFadeEvent(OnTransitionHalf);

        FadeScreen.AddOnStartFadeEvent(Character.DeactivatePlayer);
        FadeScreen.AddOnEndFadeEvent(Character.ActivatePlayer);
        PlayerInfoStorage.CurrentInfoStorage.MoveToScene();
    }

    public void FadeTo()
    {
        if (CustomTransition)
        {
            FadeScreen.SetColor(transitionColor);
            FadeScreen.SetTimes(transitionFadeTime, transitionTime);
        }
        else
        {
            FadeScreen.ResetTimes();
        }
        StartCoroutine(FadeScreen.Singleton.StartFadeAnimation(true, .1f));
    }


    public void Flash()
    {
        if (CustomTransition)
        {
            FadeScreen.SetColor(transitionColor);
            FadeScreen.SetTimes(transitionFadeTime, transitionTime);
        }
        else
        {
            FadeScreen.ResetTimes();
        }
        StartCoroutine(FadeScreen.Singleton.StartFlashAnimation(.1f));
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
