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
   
    [SerializeField] TMP_Dropdown languageDropdown;
    [SerializeField] List<GameObject> Menus;

    private void Start()
    {
        InitDropdown();

        if (UICanvas.Singleton)
        {
            Destroy(UICanvas.Singleton.gameObject);
        }

        GameSaveManager.Singleton.StartLoadingFiles();
        ContinueButton.gameObject.SetActive(GameSaveManager.Singleton.GetNumberOfSaveFiles() > 0);



    }
    private void InitDropdown()
    {
        // Clear existing options
        languageDropdown.ClearOptions();

        // Get the names of the enum values
        string[] languageNames = System.Enum.GetNames(typeof(Language));

        // Convert the enum names to dropdown options
        TMP_Dropdown.OptionData[] dropdownOptions = new TMP_Dropdown.OptionData[languageNames.Length];
        for (int i = 0; i < languageNames.Length; i++)
        {
            dropdownOptions[i] = new TMP_Dropdown.OptionData(languageNames[i]);
        }

        // Add the options to the dropdown
        languageDropdown.AddOptions(new System.Collections.Generic.List<TMP_Dropdown.OptionData>(dropdownOptions));

        languageDropdown.onValueChanged.AddListener(GameSaveManager.Singleton.OnLanguageDropdownValueChanged);
    }

   

    public void OpenLoadMenu()
    {
        foreach(Transform t in LoadMenuContainer.GetChild(0))
        {
            Destroy(t.gameObject);
        }
        LoadMenuContainer.gameObject.SetActive(true);
        int amount = GameSaveManager.Singleton.GetNumberOfSaveSlots();
        for (int i = 0; i < amount; i++)
        {
            GameObject button = Instantiate(LoadFileButtonPrefab, LoadMenuContainer.GetChild(0));
            GameSaveManager.Singleton.saves[i].saveIndex = i;
            button.GetComponent<LoadSaveButton>().SetSaveFile(GameSaveManager.Singleton.saves[i]);
            int index = i;
            button.GetComponent<Button>().onClick.AddListener(delegate { LoadGame(index); });
            if(index == 0)
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
        foreach(GameObject g in Menus)
        {
            g.SetActive(false);
        }
    }

    public void CloseLoadMenu()
    {
        foreach (Transform t in LoadMenuContainer.GetChild(0))
        {
            Destroy(t.gameObject);
        }
        LoadMenuContainer.gameObject.SetActive(false);

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
        yield return StartCoroutine(GameSaveManager.Singleton.LoadSaveFileCoroutine());
        GoToGame();
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
