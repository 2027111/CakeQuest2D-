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

        if (UICanvas.Singleton)
        {
            Destroy(UICanvas.Singleton.gameObject);
        }
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

   



    public void NewGame()
    {
        Debug.Log("Test");
        Debug.Log(FadeScreen.fading);
        playerSave.sceneName = firstScene;
        playerSave.nextRoomInfo.SetValue(firstRoom);
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
        StartCoroutine(FadeCoroutine(playerSave.sceneName));
    }
    public void QuitToDesktop()
    {
        Application.Quit();
    }



    public IEnumerator FadeCoroutine(string scene)
    {

        if (!FadeScreen.fading)
        {
            FadeScreen.StartTransition(true, Color.black, .5f);
        }
        yield return new WaitForSeconds(.5f);
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(scene);

        while (!asyncOperation.isDone)
        {
            yield return null;
        }

        if (!FadeScreen.fading)
        {
            FadeScreen.StartTransition(false, Color.black, .5f);
        }

        yield return null;





    }
}
