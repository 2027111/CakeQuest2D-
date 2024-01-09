using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] UnityEvent OnNewGame;
    [SerializeField] UnityEvent OnContinue;
    [SerializeField] PlayerStorage playerSave;
    [SerializeField] string firstScene;
    [SerializeField] RoomInfo firstRoom;
   
    [SerializeField] TMP_Dropdown languageDropdown;

    private void Start()
    {
        InitDropdown();
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

        languageDropdown.onValueChanged.AddListener(DialogueManager.Singleton.OnLanguageDropdownValueChanged);
    }

   



    public void NewGame()
    {
        playerSave.sceneName = firstScene;
        playerSave.nextRoomInfo = firstRoom;
        playerSave.forceNextChange = true;
        OnNewGame?.Invoke();
        GoToGame();
    }


    public void Continue()
    {
        OnContinue?.Invoke(); 
        GoToGame();
    }


    public void GoToGame()
    {
        playerSave.forceNextChange = true;
        SceneManager.LoadScene(playerSave.sceneName);
    }
    public void QuitToDesktop()
    {
        Application.Quit();
    }
}
