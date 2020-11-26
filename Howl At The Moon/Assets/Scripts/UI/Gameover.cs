using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gameover : MonoBehaviour
{

    public void Restart() { UnityEngine.SceneManagement.SceneManager.LoadScene(1); }

    public void LoadMenu() { UnityEngine.SceneManagement.SceneManager.LoadScene(0); }

    public void QuitGame() { Application.Quit(); }

}
