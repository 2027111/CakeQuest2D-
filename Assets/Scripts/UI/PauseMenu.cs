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
        get
        {
            if (_singleton == null)
            {
                // Load the MusicPlayer prefab from Resources
                GameObject PauseMenuPrefab = Resources.Load<GameObject>("PauseCanvas");
                if (PauseMenuPrefab != null)
                {
                    GameObject canvasInstance = Instantiate(PauseMenuPrefab);
                    Singleton = canvasInstance.GetComponent<PauseMenu>();
                    Debug.Log("PauseMenu Instantiated");
                }
                else
                {
                    Debug.LogError("PauseMenu prefab not found in Resources.");
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
                Debug.Log($"{nameof(PauseMenu)} instance already exists. Destroying duplicate!");
                Destroy(value.gameObject);
            }
        }
    }

    private bool isPaused;
    public static bool canPause = true;
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
            InputManager.inputManager.enabled = !pause;
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
        PlayerInfoStorage.CurrentInfoStorage.SetNewInformationToFile();
        GameSaveManager.Singleton?.SaveGame();
    }

    public void ReturnToTitle()
    {
        OnPausePressed(false);
        PlayerInfoStorage.CurrentInfoStorage.MoveToHomeScreenScene();
    }




}
