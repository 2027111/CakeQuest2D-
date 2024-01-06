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
        player.ChangeState(new CharacterBehaviour());
        PlayerInfoStorage storage = player.GetComponent<PlayerInfoStorage>();
        if (storage)
        {
            storage.infoStorage.sceneName = sceneToLoadName;
            storage.infoStorage.nextPosition = playerPositionOnLoad;
            storage.infoStorage.nextRoomInfo = roomOnLoadInfo;
            storage.infoStorage.facing = facing;
        }
        storage.MoveToScene();
        //SceneManager.LoadScene(sceneToLoadName);
    }



}
