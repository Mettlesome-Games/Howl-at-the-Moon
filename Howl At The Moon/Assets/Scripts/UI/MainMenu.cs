using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Author: Nate Hales
// Simple Main Menu logic besure to place the first game level as the next index in the build settings.
public class MainMenu : MonoBehaviour
{
    //Load the next scene in the the build list.
    public void PlayGame() { UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex + 1); }
    public void QuitGame() { Application.Quit(); }

}
