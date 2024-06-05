using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    private static PauseMenu _singleton;

    public static PauseMenu Singleton
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
                Debug.Log($"{nameof(PauseMenu)} instance already exists. Destroying duplicate!");
                Destroy(value.gameObject);
            }
        }
    }

    private bool isPaused;
    public static bool canPause;
    [SerializeField] GameObject pausePanel;
    public UnityEvent OnPause;



    private void Awake()
    {
        Singleton = this;
        canPause = true;
        OnPausePressed(false);
    }




    public void OnPausePressed()
    {
        if (canPause)
        {
            isPaused = !isPaused;
            OnPausePressed(isPaused);
        }
    }

    public void OnPausePressed(bool pause)
    {
        if (canPause)
        {
            isPaused = pause;
            pausePanel.SetActive(isPaused);
            Time.timeScale = isPaused ? 0f : 1;
            if (isPaused) { OnPause?.Invoke(); }
        }
    }

    public static void DisablePause(bool enabled)
    {
        canPause = enabled;
        if (Singleton.isPaused)
        {
            Singleton.OnPausePressed(false);
        }
    }

    public void Save()
    {
        PlayerInfoStorage playerInfoStorage = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInfoStorage>();
        playerInfoStorage.SetNewInformationToFile();
        GameSaveManager.Singleton?.SaveGame();
    }

    public void ReturnToTitle()
    {
        PlayerInfoStorage playerInfoStorage = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInfoStorage>();
        OnPausePressed(false);
        playerInfoStorage.MoveToHomeScreenScene();
    }




}
