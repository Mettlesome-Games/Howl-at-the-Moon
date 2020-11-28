using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VictoryScreen : MonoBehaviour
{

    public void Replay() { UnityEngine.SceneManagement.SceneManager.LoadScene(1); }

    public void LoadMenu() { UnityEngine.SceneManagement.SceneManager.LoadScene(0); }

    public void QuitGame() { Application.Quit(); }

}
