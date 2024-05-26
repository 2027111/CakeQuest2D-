using System;
using System.Collections;
using System.Collections.Generic;
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
    public bool startFadeon = false;
    public static FadeScreen Singleton
    {
        get => _singleton;
        private set
        {
            if (_singleton == null)
            {
                _singleton = value;
            }
            else if (_singleton != value)
            {
                Debug.Log($"{nameof(FadeScreen)} instance already exists. Destroying duplicate!");
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
            StartTransition(false, Color.black, 0);
        }
    }
    public static void MoveToScene(string sceneName, Color transitionColor, float fadeTime = -1)
    {

        Singleton?.SetColor(transitionColor);

        MoveToScene(sceneName, fadeTime);
    }

    public static void MoveToScene(string sceneName,float fadeTime = -1)
    {
        if (!movingScene)
        {

        Singleton?.SetTransitionTime((fadeTime > 0 ? fadeTime : .3f));
        Singleton?.FadeToScene(sceneName);
        }
    }

    private void FadeToScene(string sceneName)
    {
        StartCoroutine(FadeCoroutine(sceneName));
    }

    public static void StartTransition(bool on, Color color, float fadeTime = -1)
    {
        Singleton?.SetColor(color);

        Singleton?.StartTransition(on, fadeTime);


    }

    private void SetColor(Color color)
    {
        fadeScreen.color = color;
    }

    private void SetTransitionTime(float time)
    {
        fadeTime = time;
    }

    public void StartTransition(bool on, float fadeTime = -1)
    {

        Singleton?.SetTransitionTime((fadeTime > 0 ? fadeTime : .3f));
        StartCoroutine(StartFadeAnimation(on));




    }


    public IEnumerator FadeCoroutine(string scene)
    {


        movingScene = true;
        OnFadingStart?.Invoke();
        if (!FadeScreen.fading)
        {
            FadeScreen.StartTransition(true, Color.black, fadeTime);
        }
        yield return new WaitForSecondsRealtime(fadeTime);

        OnFadingMid?.Invoke();
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(scene);

        while (!asyncOperation.isDone)
        {
            yield return null;
        }
        yield return new WaitForSecondsRealtime(fadeTime);
        if (!FadeScreen.fading)
        {
            FadeScreen.StartTransition(false, Color.black, fadeTime);
        }

       
        OnFadingEnd?.Invoke();
        movingScene = false;
        yield return null;





    }



    public IEnumerator StartFadeAnimation(bool on)
    {
        float time = 0f;
        float start = on?0:1;
        float target = on?1:0;

        group.alpha = start;
        fading = true;
        while (time < fadeTime)
        {
            time += Time.deltaTime;
            group.alpha = Mathf.Lerp(start, target, time/ fadeTime);
            yield return null;
        }
        group.alpha = target;
        if(group.alpha == 1)
        {
            fadeOn = true;
        }
        else
        {
            fadeOn = false;
        }
        fading = false;

        yield return null;
    }
}
