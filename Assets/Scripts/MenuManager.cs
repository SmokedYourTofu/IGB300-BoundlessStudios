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
    public int gameScene2 = 14;
    public int tutScene = 3;
    public bool isPaused;

    public GameObject pauseMenu;
    public GameObject UI;
    public GameObject GameManager;
    public GameObject leveltwo;

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
        if (GameManager)
        {
            Destroy(GameManager);
        }
        SceneManager.LoadScene(gameScene);
        Time.timeScale = 1.0f;
        isPaused = false;
    }

    public void StartLevel2()
    {
        if (GameManager)
        {
            Destroy(GameManager);
        }
        SceneManager.LoadScene(gameScene2);
        Time.timeScale = 1.0f;
        isPaused = false;
    }

    public void MainMenu()
    {
        if (GameManager)
        {
            Destroy(GameManager);
        }
        Time.timeScale = 0.0f;
        SceneManager.LoadScene(mainMenu);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void Restart()
    {
        if (GameManager)
        {
            Destroy(GameManager);
        }
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
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

    public void StartTut()
    {
        SceneManager.LoadScene(tutScene);
        Time.timeScale = 1.0f;
        isPaused = false;
    }

    public void Levels()
    {
        if (GameManager)
        {
            Destroy(GameManager);
        }
        Time.timeScale = 0.0f;
        SceneManager.LoadScene("Levels", LoadSceneMode.Additive);
    }

    public void CloseLevels()
    {
        Time.timeScale = 1.0f;
        SceneManager.UnloadSceneAsync("Levels");
    }
}