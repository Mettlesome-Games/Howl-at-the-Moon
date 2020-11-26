using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    [SerializeField]GameObject uiPauseMenu, pauseGroup, optionsMenu;


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
        Gamemanager.instance.ResumeGame();
    }
    public void Pause() 
    { 
        uiPauseMenu.SetActive(true);
        pauseGroup.SetActive(true);
        optionsMenu.SetActive(false);
        Gamemanager.instance.PauseGame();
    }

    public void LoadMenu() { UnityEngine.SceneManagement.SceneManager.LoadScene(0); }

    public void QuitGame() { Application.Quit(); }

}
