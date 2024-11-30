using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] UnityEvent OnNewGame;
    [SerializeField] UnityEvent OnContinue;
    [SerializeField] PlayerStorage playerSave;
    [SerializeField] Button NewGameButton;
    [SerializeField] Button ContinueButton;
    [SerializeField] GameObject LoadFileButtonPrefab;
    [SerializeField] Transform LoadMenuContainer;
    [SerializeField] string firstScene;
    [SerializeField] RoomInfo firstRoom;
    public bool Loading = false;
    [SerializeField] TMP_Dropdown languageDropdown;
    [SerializeField] List<UIMenu> Menus;

    private void Start()
    {
        CloseAll();

        if (UICanvas.Exists())
        {
            Destroy(UICanvas.Singleton.gameObject);
        }

        GameSaveManager.Singleton.StartLoadingFiles(delegate
        {
            OpenMenu(0);

            ContinueButton.gameObject.SetActive(GameSaveManager.Singleton.GetNumberOfSaveFiles() > 0);
        });


    }

    public void DisableButtons()
    {
        Loading = true;
    }

    public void OpenLoadMenu()
    {
        OpenMenu(1);
        foreach (Transform t in LoadMenuContainer)
        {
            Destroy(t.gameObject);
        }
        int amount = GameSaveManager.Singleton.GetNumberOfSaveSlots();
        for (int i = 0; i < amount; i++)
        {
            GameObject button = Instantiate(LoadFileButtonPrefab, LoadMenuContainer);
            GameSaveManager.Singleton.saves[i].saveIndex = i;
            button.GetComponent<LoadSaveButton>().SetSaveFile(GameSaveManager.Singleton.saves[i]);
            int index = i;
            button.GetComponent<Button>().onClick.AddListener(delegate { LoadGame(index); });
            if (index == 0)
            {
                button.GetComponent<Button>().Select();
            }
        }
    }



    private void LoadGame(int index)
    {
        GameSaveManager.Singleton.SetSaveFileIndex(index);
        StartCoroutine(StartGameRoutine());
    }


    public void CloseAll()
    {
        foreach (UIMenu g in Menus)
        {
            g.CloseMenu();
        }
    }


    public void OpenMenu(int index)
    {
        Menus[index].OpenMenu();
    }


    public void CloseLoadMenu()
    {
        foreach (Transform t in LoadMenuContainer)
        {
            Destroy(t.gameObject);
        }

    }
    public void NewGame()
    {

        GameSaveManager.Singleton?.CreateNewSaveSlot();
        StartCoroutine(StartGameRoutine());
    }


    public void Continue()
    {
        OnContinue?.Invoke();
        GoToGame();
    }

    public IEnumerator StartGameRoutine()
    {
        if (!Loading)
        {
            DisableButtons();
            yield return StartCoroutine(GameSaveManager.Singleton.LoadSaveFileCoroutine());
            GoToGame();
        }
    }
    public void GoToGame()
    {
        playerSave.forceNextChange = true;

        FadeScreen.MoveToScene(playerSave.sceneName, Color.black, .3f);
    }
    public void QuitToDesktop()
    {
        Application.Quit();
    }



}
