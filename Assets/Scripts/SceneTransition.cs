using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{


    public string sceneToLoadName;
    public Vector2 playerPositionOnLoad;
    public RoomInfo roomOnLoadInfo;
    public GameObject fadeInPanel;
    public GameObject fadeOutPanel;
    Character player;

    public float minimumFadeTime = 1f;
    public static float DestroyTime = 1.4f;


    private void Awake()
    {
        if(fadeInPanel != null)
        {
            GameObject panel = Instantiate(fadeInPanel, Vector3.zero, Quaternion.identity) as GameObject;
            Destroy(panel, DestroyTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            
            player = other.GetComponent<Character>();
            player.ChangeState(new CharacterBehaviour());
            PlayerInfoStorage storage = player.GetComponent<PlayerInfoStorage>();
            if (storage)
            {
                storage.infoStorage.nextPosition = playerPositionOnLoad;
                storage.infoStorage.nextRoomInfo = roomOnLoadInfo;
                storage.infoStorage.forceNewInfo = true;
            }
            StartCoroutine(FadeCoroutine());
            //SceneManager.LoadScene(sceneToLoadName);

        }
    }

    public IEnumerator FadeCoroutine()
    {
        if (fadeOutPanel != null)
        {
            Instantiate(fadeOutPanel, Vector3.zero, Quaternion.identity);
        }
        yield return new WaitForSeconds(minimumFadeTime);
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneToLoadName);

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
