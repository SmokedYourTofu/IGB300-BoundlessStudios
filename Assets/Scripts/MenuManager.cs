using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public int mainMenu = 0;
    public int levelsScene = 1;
    public int gameScene = 2;
    public bool isPaused;
    

    public GameObject pauseMenu;
    public GameObject UI;

    // Update is called once per frame
    void Update()
    {
       if (Input.GetKeyDown(KeyCode.Escape))
        {
            if(isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    public void StartGame()
    {
        SceneManager.LoadScene(gameScene);
        Time.timeScale = 1.0f;
        isPaused = false;
    }

    public void MainMenu()
    {
        Time.timeScale = 0.0f;
        SceneManager.LoadScene(mainMenu);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void Restart()
    {
        SceneManager.LoadScene(gameScene);
        Time.timeScale = 1.0f;
        isPaused = false;
    }

    public void PauseGame()
    {
        pauseMenu.SetActive(true);
        UI.SetActive(false);
        Time.timeScale = 0.0f;
        isPaused = true;
    }

    public void ResumeGame()
    {
        pauseMenu.SetActive(false);
        UI.SetActive(true);
        Time.timeScale = 1.0f;
        isPaused = false;
    }

    public void Levels()
    {
        Time.timeScale = 0.0f;
        SceneManager.LoadScene("Levels", LoadSceneMode.Additive);
    }

    public void CloseLevels()
    {
        Time.timeScale = 1.0f;
        SceneManager.UnloadSceneAsync("Levels");
    }
}