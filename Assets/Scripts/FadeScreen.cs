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

        if (FadeScreen.fading)
        {
            Singleton.StopCoroutine("FadeCoroutine");
            fading = false;
            fadeOn = false;
            SetAlphaTarget(0);
        }
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
        yield return new WaitForSeconds(.05f);
        OnFadingStart?.Invoke();

        if (!FadeScreen.fading && !fadeOn)
        {
            Singleton.SetColor(Color.black);
            Singleton.SetTransitionTime(fadeTime);
            yield return Singleton.StartCoroutine(StartFadeAnimation(true));
            fadeOn = true;
        }
        yield return new WaitForSeconds(.05f);
        OnFadingMid?.Invoke();
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(scene);

        while (!asyncOperation.isDone)
        {
            yield return null;
        }
        yield return new WaitForSecondsRealtime(fadeTime);
        if (!FadeScreen.fading && fadeOn)
        {

            Singleton.SetColor(Color.black);
            Singleton.SetTransitionTime(fadeTime);
            yield return Singleton.StartCoroutine(StartFadeAnimation(false));
            fadeOn = false;
        }

        yield return new WaitForSeconds(.05f);

        OnFadingEnd?.Invoke();
        movingScene = false;
        yield return null;


        OnFadingStart?.RemoveAllListeners();
        OnFadingMid?.RemoveAllListeners();
        OnFadingEnd?.RemoveAllListeners();





    }

    public void SetAlphaTarget(float target)
    {
        group.alpha = target;
    }



    public IEnumerator StartFadeAnimation(bool on)
    {
        float time = 0f;
        float start = on?0:1;
        float target = on?1:0;

        group.alpha = start;
        SetAlphaTarget(start);
        fading = true;
        while (time < fadeTime)
        {
            time += Time.deltaTime;
            group.alpha = Mathf.Lerp(start, target, time/ fadeTime);
            yield return null;
        }
        SetAlphaTarget(target);
        if (group.alpha == 1)
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
