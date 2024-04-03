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
    Character player;


    private void Start()
    {

        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Character>();
    }

    public void TransitionScene()
    {
        Debug.Log("Scene Transition Started");
        player.ChangeState(new CharacterBehaviour());
        PlayerInfoStorage storage = player.GetComponent<PlayerInfoStorage>();
        if (storage)
        {
            storage.infoStorage.sceneName = sceneToLoadName;
            storage.infoStorage.nextPosition = playerPositionOnLoad;
            storage.infoStorage.facing = facing;
        }
        FadeScreen.Singleton?.OnFadingMid.AddListener(OnTransitionHalf);
        storage.MoveToScene();
        //SceneManager.LoadScene(sceneToLoadName);
    }




    public void OnTransitionHalf()
    {
        PlayerInfoStorage storage = player.GetComponent<PlayerInfoStorage>();
        storage.infoStorage.nextRoomInfo.SetValue(roomOnLoadInfo);


        FadeScreen.Singleton?.OnFadingMid.RemoveListener(OnTransitionHalf);

    }


}
