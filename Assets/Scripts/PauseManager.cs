using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
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
    public UnityEvent OnPause;

    public RectTransform mainPanel;
    public RectTransform[] menuPanels;
    public float slideSpeed = 5f;

    private int currentIndex = 0;

    private void Awake()
    {
        Singleton = this;
        canPause = true;
        OnPausePressed(false);
    }


    public void SlideToPanel(int index)
    {
        if (index >= 0 && index < menuPanels.Length)
        {
            currentIndex = index;
            UpdatePanelPositions();
        }
    }

    void SlideToNextPanel()
    {
        if (currentIndex < menuPanels.Length - 1)
        {
            currentIndex++;
            UpdatePanelPositions();
        }
    }

    void SlideToPreviousPanel()
    {
        if (currentIndex > 0)
        {
            currentIndex--;
            UpdatePanelPositions();
        }
    }

    void UpdatePanelPositions()
    {
        for (int i = 0; i < menuPanels.Length; i++)
        {
            menuPanels[i].position = i == currentIndex ? mainPanel.transform.position : mainPanel.transform.position    + new Vector3(1300, 0, 0);

        }
    }

    public void OnPausePressed()
    {
        if (canPause)
        {
            Debug.Log("Pause");
            isPaused = !isPaused;
            pausePanel.SetActive(isPaused);
            Time.timeScale = isPaused ? 0f : 1;
            if (isPaused) { OnPause?.Invoke(); }
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

        GameSaveManager.Singleton?.SaveScriptables();
    }

    public void ReturnToTitle()
    {
        SceneManager.LoadScene("StartMenuScene");
    }
}
