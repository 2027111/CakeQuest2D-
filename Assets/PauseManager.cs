using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{

    private static PauseManager _singleton;

    public static PauseManager Singleton
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
                Debug.Log($"{nameof(PauseManager)} instance already exists. Destroying duplicate!");
                Destroy(value.gameObject);
            }
        }
    }
    private bool isPaused;
    public static bool canPause;
    [SerializeField] GameObject pausePanel;
    private void Start()
    {
        Singleton = this;
        OnPausePressed(false);
    }

    public void OnPausePressed()
    {
        OnPausePressed(!isPaused);
    }


    public void OnPausePressed(bool pause)
    {
        isPaused = pause;
        pausePanel.SetActive(isPaused);
        Time.timeScale = isPaused ? 0f : 1;
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

        GameSaveManager.Singleton.SaveScriptables();
    }

    public void ReturnToTitle()
    {
        SceneManager.LoadScene("StartMenuScene");
    }
    
}
