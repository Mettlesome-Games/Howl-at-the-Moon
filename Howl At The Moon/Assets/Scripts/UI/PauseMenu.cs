using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    [SerializeField]GameObject uiPauseMenu, pauseGroup, optionsMenu;
    [Header("Grid stuff:")]
    [SerializeField] GameObject mainGrid;
    [SerializeField] GameObject tipBar;

    private void Update()
    {
        if (Input.GetButtonDown("Cancel")) 
        {
            if (Gamemanager.GameIsPaused)
            {
                Resume();
            }
            else 
            {
                Pause();
            }
        }
    }

    public void Resume() 
    {
        uiPauseMenu.SetActive(false);
        pauseGroup.SetActive(true);
        optionsMenu.SetActive(false);
        mainGrid.SetActive(true);
        tipBar.SetActive(true);
        Gamemanager.instance.ResumeGame();
    }
    public void Pause() 
    { 
        uiPauseMenu.SetActive(true);
        pauseGroup.SetActive(true);
        optionsMenu.SetActive(false);
        mainGrid.SetActive(false);
        tipBar.SetActive(false);
        Gamemanager.instance.PauseGame();
    }

    public void LoadMenu() 
    {
        Gamemanager.instance.ResumeGame();
        UnityEngine.SceneManagement.SceneManager.LoadScene(0); 
    }

    public void QuitGame() { Application.Quit(); }

}
