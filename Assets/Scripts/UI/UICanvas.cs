using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class UICanvas : MonoBehaviour
{

    private static UICanvas _singleton;
    public static UICanvas Singleton
    {
        get
        {
            if (_singleton == null)
            {
                // Load the MusicPlayer prefab from Resources
                GameObject canvasPrefab = Resources.Load<GameObject>("UICanvas");
                if (canvasPrefab != null)
                {
                    GameObject canvasInstance = Instantiate(canvasPrefab);
                    Singleton = canvasInstance.GetComponent<UICanvas>();
                    Debug.Log("UICanvas Instantiated");
                }
                else
                {
                    Debug.LogError("UICanvas prefab not found in Resources.");
                }
            }
            return _singleton;
        }
        private set
        {
            if (_singleton == null)
            {
                _singleton = value;
            }
            else if (_singleton != value)
            {
                Debug.Log($"{nameof(UICanvas)} instance already exists. Destroying duplicate!");
                Destroy(value.gameObject);
            }
        }
    }



    void Awake()
    {
        if (_singleton == null)
        {
            Singleton = this;
            DontDestroyOnLoad(Singleton.gameObject);
        }
        else if (_singleton != this)
        {
            Destroy(this.gameObject);
        }
    }



    [SerializeField] UIBorder border;
    [SerializeField] QuestList questList;
    [SerializeField] PartyList partyList;
    [SerializeField] DialogueBox dialogueBox;
    [SerializeField] VideoPlayer videoPlayer;


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            PlayVideoRequest();
        }
    }


    public void PlayVideoRequest()
    {
        GameObject.FindGameObjectWithTag("Player").GetComponent<Character>().ToggleCutsceneState();
        videoPlayer.prepareCompleted += delegate
        {
            StartCoroutine(StartAnimatedCutscene());
        };
        videoPlayer.Prepare();


    }


    public IEnumerator StartAnimatedCutscene()
    {
        
        TurnBordersOn(false);
        yield return dialogueBox.WaitForResume();

        FadeScreen.Singleton.SetColor(Color.black);
        yield return FadeScreen.Singleton.StartFadeAnimation(true);
        yield return ShowVideo(true);


        MusicPlayer.Stop();

        InputManager.inputManager.OnPausedPressed = null;
        videoPlayer.loopPointReached += delegate { EndVideo(); };
        InputManager.inputManager.OnPausedPressed += delegate { EndVideo(); };
        PlayVideo();
        yield return FadeScreen.Singleton.StartFadeAnimation(false);
        yield return new WaitForSeconds(.1f); //Let the video start yaknow;







    }



    public IEnumerator EndAnimatedCutscene()
    {


        yield return new WaitForSeconds(.1f); //Let the video end yaknow;



        yield return FadeScreen.Singleton.StartFadeAnimation(true);
        yield return ShowVideo(false);
        yield return dialogueBox.Resume();
        MusicPlayer.Resume();
        TurnBordersOn(true);
        videoPlayer.Stop();
        GameObject.FindGameObjectWithTag("Player").GetComponent<Character>().TogglePreviousState();
        yield return FadeScreen.Singleton.StartFadeAnimation(false);
    }

    public void EndVideo()
    {
        videoPlayer.loopPointReached -= delegate { EndVideo(); };
        InputManager.inputManager.OnPausedPressed = null;
        videoPlayer.Pause();
        StartCoroutine(EndAnimatedCutscene());
    }
    public void PlayVideo()
    {
        videoPlayer.targetCamera = Camera.main;
        videoPlayer.Play();
    }

    private IEnumerator ShowVideo(bool showVideo)
    {

            float target = showVideo ? 1 : 0;
            float start = videoPlayer.targetCameraAlpha;
        if(target != start)
        {
            
            float duration = 0;
            while (duration < .5f)
            {
                videoPlayer.targetCameraAlpha = Mathf.Lerp(start, target, duration / .5f);
                duration += Time.deltaTime;
                yield return null;
            }
            videoPlayer.targetCameraAlpha = target;

        }

    }



    public static void StartDialogue(Dialogue dialogue, GameObject playerObject = null, GameObject originObject = null, GameState state = GameState.Overworld)
    {
        if (!FadeScreen.fading)
        {
            Singleton?.dialogueBox.StartDialogue(dialogue, playerObject, originObject, state);
        }
    }
    public static void ForceStopDialogue()
    {
        Singleton?.dialogueBox.ForceStop();
    }

    public static bool DialogueBoxIsActive()
    {
        return Singleton.dialogueBox.IsActive();
    }

    public static void CancelCurrentDialogue()
    {

        Singleton?.dialogueBox.CancelDialogue();
    }


    public static void TurnBordersOn(bool on)
    {
        Singleton?.border.Appear(on);
        Singleton?.questList.Appear(on);
        Singleton?.partyList.Appear(on);
    }

    public static void UpdateQuestList()
    {
        Singleton?.questList?.ResetList();
    }

    public static void UpdatePartyList()
    {
        Singleton?.partyList?.UpdateList();
    }
}
