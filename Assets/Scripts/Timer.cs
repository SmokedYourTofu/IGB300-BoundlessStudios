using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.SceneManagement;

public class Timer : MonoBehaviour
{
    public float TimeLeft;
    public bool TimerOn = false;
    public GameObject endgame;
    public GameObject levelbutton;

    private TMP_Text timerText;

    public AudioSource music;

    // Start is called before the first frame update
    void Start()
    {
        timerText = this.GetComponent<TMP_Text>();
        TimerOn = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (TimerOn)
        {
            if (TimeLeft > 0)
            {
                TimeLeft -= Time.deltaTime;
                updateTimer(TimeLeft);
            }
            else
            {
                Debug.Log("Time Up");
                endgame.SetActive(true);
                TimeLeft = 0;
                TimerOn = false;
                SendFinalScoreToDiscord();
                UnloadAllAdditiveScenes();
                nextlevel();

            }

            if (TimeLeft <= 30)
            {
                music.pitch = 1.25f;
                var ratio = Mathf.Abs(Mathf.Sin(Time.time * 2f));
                timerText.color = Color.Lerp(Color.white, Color.red, ratio);
            }
        }
    }

    void updateTimer(float currentTime)
    {
        currentTime += 1;

        float minutes = Mathf.FloorToInt(currentTime / 60);
        float seconds = Mathf.FloorToInt(currentTime % 60);

        timerText.text = string.Format("{0:00} : {1:00}", minutes, seconds);
    }

    void SendFinalScoreToDiscord()
    {
        float finalScore = ScoreManager.Instance.TotalScore;
        string message = $"Time is up! Final Player Score: {finalScore:F1}";
        DiscordWebhooks.SendMessage(message);
        //DiscordWebhooks.SendScreenshot();
        Debug.Log(message);
    }

    // This method will unload all scenes except the active one
    public void UnloadAllAdditiveScenes()
    {
        // Get the active scene (the base scene)
        Scene activeScene = SceneManager.GetActiveScene();

        // Iterate through all loaded scenes
        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            Scene scene = SceneManager.GetSceneAt(i);

            // Skip the active scene (main base scene)
            if (scene != activeScene)
            {
                // Unload the scene asynchronously
                StartCoroutine(UnloadScene(scene.name));
            }
        }
    }

    // Helper method to unload a scene asynchronously
    private IEnumerator UnloadScene(string sceneName)
    {
        AsyncOperation asyncUnload = SceneManager.UnloadSceneAsync(sceneName);
        while (!asyncUnload.isDone)
        {
            yield return null; // Wait until the scene is completely unloaded
        }
        Debug.Log("Scene " + sceneName + " unloaded.");
    }

    public void nextlevel()
    {
        if(levelbutton != null)
        {
            float score = ScoreManager.Instance.TotalScore;
            if (score > 1000) 
            {
                levelbutton.SetActive(true);
            }
            else
            {
                levelbutton.SetActive(false);
            }
        }
    }
}
