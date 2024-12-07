using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FadeScreen : MonoBehaviour
{
    [SerializeField] Image fadeScreen;
    [SerializeField] CanvasGroup group;

    public static bool fading = false;
    public static bool fadeOn = false;
    public static bool movingScene = false;
    private static FadeScreen _singleton;




    private float fadeTime = .5f;
    private float WaitTime = 1f;
    public bool startFadeon = false;
    public static FadeScreen Singleton
    {
        get
        {
            if (_singleton == null)
            {
                // Load the MusicPlayer prefab from Resources
                GameObject FadeScreenPrefab = Resources.Load<GameObject>("FadeScreen");
                if (FadeScreenPrefab != null)
                {
                    GameObject FadeScreenInstance = Instantiate(FadeScreenPrefab);
                    Singleton = FadeScreenInstance.GetComponent<FadeScreen>();
                    // Debug.Log("FadeScreen Instantiated");
                }
                else
                {
                    Debug.LogError("FadeScreen prefab not found in Resources.");
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
                Debug.LogWarning($"{nameof(FadeScreen)} instance already exists. Destroying duplicate!");
                Destroy(value.gameObject);
            }
        }
    }



    public UnityEvent OnFadingStart;
    public UnityEvent OnFadingMid;
    public UnityEvent OnFadingEnd;

    private void Awake()
    {
        Singleton = this;
        DontDestroyOnLoad(gameObject);
    }
    private void Start()
    {
        if (startFadeon)
        {

            fadeOn = true;
            ResetTimes();
            StartTransition(false);
        }
    }
    public static void MoveToScene(string sceneName, Color transitionColor, float fadeTime = -1)
    {

        SetColor(transitionColor);

        MoveToScene(sceneName);
    }

    public static void ResetTimes()
    {
        Singleton.fadeTime = .5f;
        Singleton.WaitTime = 1f;
        Singleton.fadeScreen.color = Color.black;

    }



    public static void MoveToScene(string sceneName)
    {
        if (!movingScene)
        {
            Singleton?.FadeToScene(sceneName);
        }
    }

    private void FadeToScene(string sceneName)
    {

        if (FadeScreen.fading)
        {
            Singleton.StopCoroutine("FadeCoroutine");
            fading = false;
            fadeOn = false;
            SetAlphaTarget(0);
        }
        StartCoroutine(FadeCoroutine(sceneName));
    }

    public static void StartTransition(bool on)
    {

        Singleton?.StartTransition(on);


    }
    public static void SetTimes(float transitionFadeTime, float transitionTime)
    {
        Singleton?.SetTransitionTime(transitionFadeTime);
        Singleton?.SetTransitionWaitTime(transitionTime);

    }

    private void SetTransitionWaitTime(float transitionTime)
    {
        WaitTime = transitionTime;
    }

    public static void SetColor(Color color)
    {
        Singleton.fadeScreen.color = color;
    }

    public void SetTransitionTime(float time)
    {
        fadeTime = time;
    }

    public void StartTransition(bool on, float fadeTime = -1, float waitTime = 0)
    {

        Singleton?.SetTransitionTime((fadeTime > 0 ? fadeTime : .8f));
        StartCoroutine(StartFadeAnimation(on, Singleton.fadeTime, waitTime));




    }

    public static void FakeMoveToScene()
    {
        Singleton?.StartCoroutine(Singleton?.FadeCoroutine());
    }


    public IEnumerator FadeCoroutine(string scene = null)
    {


        movingScene = true;
        yield return new WaitForSeconds(.05f);
        OnFadingStart?.Invoke();

        if (!FadeScreen.fading)
        {
            Singleton.SetTransitionTime(fadeTime);
            yield return Singleton.StartCoroutine(StartFadeAnimation(true, fadeTime));
            fadeOn = true;
        }
        yield return new WaitForSeconds(.05f);
        OnFadingMid?.Invoke();
        if(scene != null)
        {
            AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(scene);

            while (!asyncOperation.isDone)
            {
                yield return null;
            }
            yield return new WaitForSecondsRealtime(WaitTime);
        }
      
        if (!FadeScreen.fading)
        {

            Singleton.SetTransitionTime(fadeTime);
            yield return Singleton.StartCoroutine(StartFadeAnimation(false, fadeTime));
            fadeOn = false;
        }

        yield return new WaitForSeconds(.05f);

        OnFadingEnd?.Invoke();
        movingScene = false;
        yield return null;


        OnFadingStart?.RemoveAllListeners();
        OnFadingMid?.RemoveAllListeners();
        OnFadingEnd?.RemoveAllListeners();

        ResetTimes();



    }

    public void SetAlphaTarget(float target)
    {
        group.alpha = target;
    }



    public IEnumerator StartFadeAnimation(bool on, float fadeTime = .8f, float waitTime = 0)
    {
        float time = 0f;
        float start = on ? 0 : 1;
        float target = on ? 1 : 0;
        SetAlphaTarget(start);
        fading = true;
        yield return new WaitForSeconds(waitTime);
        while (time < fadeTime)
        {
            time += Time.deltaTime;
            group.alpha = Mathf.Lerp(start, target, time / fadeTime);
            yield return null;
        }
        SetAlphaTarget(target);
        fading = false;


        yield return null;
    }
    public IEnumerator StartFlashAnimation(float fadeTime = .8f, float fadeDuration = .2f, float waitTime = 0)
    {
        float time = 0f;
        float start = 0;
        float target = 1;
        SetAlphaTarget(start);
        fading = true;
        yield return new WaitForSeconds(waitTime);
        while (time < fadeTime)
        {
            time += Time.deltaTime;
            group.alpha = Mathf.Lerp(start, target, time / fadeTime);
            yield return null;
        }
        SetAlphaTarget(target);
        yield return new WaitForSeconds(fadeDuration);

        time = 0f;
        while (time < fadeTime)
        {
            time += Time.deltaTime;
            group.alpha = Mathf.Lerp(target, start, time / fadeTime);
            yield return null;
        }

        SetAlphaTarget(start);

        fading = false;
        yield return null;
    }

    public static void AddOnMidFadeEvent(Action action)
    {
        UnityAction actionU = new UnityAction(action);
        Debug.Log(action.Method.Name);
        Singleton?.OnFadingMid.AddListener(actionU);
    }

    public static void AddOnStartFadeEvent(Action action)
    {
        UnityAction actionU = new UnityAction(action);
        Singleton?.OnFadingStart.AddListener(actionU);
    }

    public static IEnumerable<Task> WaitForNotFade()
    {
        while (fading)
        {
            yield return null;
        }
        yield return null;

    }

    public static void AddOnEndFadeEvent(Action action)
    {
        UnityAction actionU = new UnityAction(action);

        if (Singleton.startFadeon)
        {
            actionU?.Invoke();
        }
        else
        {
            Singleton?.OnFadingEnd.AddListener(actionU);
        }
    }

    public static void RemoveOnEndFadeEvent(Action action)
    {
        UnityAction actionU = new UnityAction(action);
        Singleton?.OnFadingEnd.RemoveListener(actionU);

    }
}
