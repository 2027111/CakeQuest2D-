using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] UnityEvent OnNewGame;
    [SerializeField] UnityEvent OnContinue;
    [SerializeField] PlayerStorage playerSave;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void NewGame()
    {
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
