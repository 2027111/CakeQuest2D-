using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerInfoStorage : MonoBehaviour
{


    public PlayerStorage infoStorage;
    [SerializeField] Character character;


    public GameObject fadeInPanel;
    public GameObject fadeOutPanel;
    public float minimumFadeTime = 1f;
    public static float DestroyTime = 1.4f;

    



    // Start is called before the first frame update
    void Start()
    {
        
        character = GetComponent<Character>();

        if (infoStorage)
        {
            if (infoStorage.forceNextChange)
            {
                MoveToScene();
            }
        }
        if (fadeInPanel != null)
        {
            GameObject panel = Instantiate(fadeInPanel, Vector3.zero, Quaternion.identity) as GameObject;
            Destroy(panel, DestroyTime);
        }
    }


    public void GoToBattleScene()
    {
        SetNewInformationToFile();
        infoStorage.forceNextChange = true;
        StartCoroutine(FadeCoroutine("BattleScene"));

    }

    public void MoveToScene()
    {



        if (DialogueBox.Singleton)
        {
            if (DialogueBox.Singleton.IsActive())
            {
                DialogueBox.Singleton.ForceStop();
            }
        }
        if (infoStorage)
        {

            if (infoStorage.sceneName != SceneManager.GetActiveScene().name)
            {
                infoStorage.forceNextChange = true;
                StartCoroutine(FadeCoroutine(infoStorage.sceneName));
            }
            else
            {
                if (fadeInPanel != null)
                {
                    GameObject panel = Instantiate(fadeInPanel, Vector3.zero, Quaternion.identity) as GameObject;
                    Destroy(panel, DestroyTime);
                }

                if (infoStorage.nextRoomInfo != Camera.main.GetComponent<CameraMovement>().GetCurrentRoom())
                {
                    Camera.main.GetComponent<CameraMovement>().SetNewRoom(infoStorage.nextRoomInfo);
                }

                character.SetPosition(infoStorage.nextPosition);
                Camera.main.GetComponent<CameraMovement>().ForceToTarget();

                infoStorage.forceNextChange = false;

                character.LookToward(RoomMove.DirectionToVector(infoStorage.facing));

            }
        }
        
    }

    public void SetNewInformationToFile()
    {
        infoStorage.nextPosition = transform.position;
        infoStorage.nextRoomInfo = Camera.main.GetComponent<CameraMovement>().currentRoomInfo;
        infoStorage.sceneName = SceneManager.GetActiveScene().name;
    }

    public void SetNewPosition(Vector2 newPos)
    {
        infoStorage.nextPosition = newPos;
    }

    public void SetNewRoom(RoomInfo newRoom)
    {
        infoStorage.nextRoomInfo = newRoom;
    }

    public IEnumerator FadeCoroutine(string scene)
    {

        GetComponent<Character>().ChangeState(new NothingBehaviour());
        if (fadeOutPanel != null)
        {
            Instantiate(fadeOutPanel, Vector3.zero, Quaternion.identity);
        }
        yield return new WaitForSeconds(minimumFadeTime);
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(scene);

        while (!asyncOperation.isDone)
        {
            yield return null;
        }

        if (fadeInPanel != null)
        {
            GameObject panel = Instantiate(fadeInPanel, Vector3.zero, Quaternion.identity) as GameObject;
            Destroy(panel, DestroyTime);
        }



        yield return null;





    }
}
