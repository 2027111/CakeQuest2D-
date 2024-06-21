using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class MoviePlayer : MonoBehaviour
{

    [SerializeField] VideoPlayer videoPlayer;
    public UnityEvent OnVideoOver;
    public float fadeTime = .3f;
    public bool automatic;


    private void Start()
    {
        if (automatic)
        {
            PlayVideoRequest();
        }
    }


    public void PlayVideoRequest()
    {


        if (!videoPlayer.isPlaying)
        {
            videoPlayer.prepareCompleted += delegate
            {
                StartCoroutine(StartAnimatedCutscene());
            };
            videoPlayer.Prepare();
        }


    }


    public IEnumerator StartAnimatedCutscene()
    {


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
        videoPlayer.Stop();
        yield return FadeScreen.Singleton.StartFadeAnimation(false);
        OnVideoOver?.Invoke();
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
        if (target != start)
        {

            float duration = 0;
            while (duration < fadeTime)
            {
                videoPlayer.targetCameraAlpha = Mathf.Lerp(start, target, duration / fadeTime);
                duration += Time.deltaTime;
                yield return null;
            }
            videoPlayer.targetCameraAlpha = target;

        }

    }

    public void NextScene()
    {
        FadeScreen.MoveToScene("StartMenuScene");
    }






}
