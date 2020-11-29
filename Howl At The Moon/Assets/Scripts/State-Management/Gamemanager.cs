using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Gamemanager : MonoBehaviour
{
   // When Called if the instance is null spawn and assign a new instance.
    #region Singelton
    private static Gamemanager _instance;
    public static Gamemanager instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject me = new GameObject("GameManager");
                me.AddComponent<Gamemanager>();
            }
            return _instance;
        }
    }
    #endregion

    public static bool GameIsPaused = false;

    private void Awake()
    {
        if (_instance == null) _instance = this; //Assign yourself
        else Destroy(gameObject);
        //DontDestroyOnLoad(gameObject);
    }

    public void hello()
    {
        print("GM Reporting! " + this.name);
    }

    public void LoseGame()
    { 
        UnityEngine.SceneManagement.SceneManager.LoadScene(3); 
    }

    public void WinGame()
    {  
        UnityEngine.SceneManagement.SceneManager.LoadScene(2); 
    }

    //Use to Switch Scenes
    public void LoadLevel(string SceneName)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(SceneName);
    }

    public void PauseGame() { 
        Time.timeScale = 0.0f; 
        GameIsPaused = true; 
    }
    public void ResumeGame() { 
        Time.timeScale = 1.0f;
        GameIsPaused = false;
    }
    
}
