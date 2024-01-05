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
    }

    public void MoveToScene()
    {
        if (infoStorage)
        {

            if (infoStorage.sceneName != SceneManager.GetActiveScene().name)
            {
                infoStorage.forceNextChange = true;
                StartCoroutine(FadeCoroutine());
            }
            else
            {
                if (infoStorage.nextRoomInfo != Camera.main.GetComponent<CameraMovement>().GetCurrentRoom())
                {
                    Camera.main.GetComponent<CameraMovement>().SetNewRoom(infoStorage.nextRoomInfo);
                }
                else
                {



                
                    if (fadeInPanel != null)
                    {
                        GameObject panel = Instantiate(fadeInPanel, Vector3.zero, Quaternion.identity) as GameObject;
                        Destroy(panel, DestroyTime);
                    }
                }

                character.SetPosition(infoStorage.nextPosition);
                Camera.main.GetComponent<CameraMovement>().ForceToTarget();

                infoStorage.forceNextChange = false;
            }

            character.LookToward(RoomMove.DirectionToVector(infoStorage.facing));

        }
        
    }

    public IEnumerator FadeCoroutine()
    {
        if (fadeOutPanel != null)
        {
            Instantiate(fadeOutPanel, Vector3.zero, Quaternion.identity);
        }
        yield return new WaitForSeconds(minimumFadeTime);
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(infoStorage.sceneName);

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
