using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MazeContoller : MonoBehaviour
{

    public AudioSource[] audioSources;

    private MiniGameSpawner mySpawner;
    public GameObject completeText;

    // Score parameters
    public TMP_Text scoreText; // Reference to the TMP Text component for displaying score
    public float totalTime = 30f; // Total time for the game (example value)
    public float basePoints = 100f; // Base number of points (example value)
    public float progressiveMultiplierMin = 0.1f; // Minimum progressive multiplier
    public float progressiveMultiplierMax = 2f; // Maximum progressive multiplier
    public float gameMultiplier = 0.15f; // Game-specific multiplier
    private float timeRemaining; // Time remaining for this game
    private bool isGameCompleted = true; // Indicates if the game was completed
    private bool isSuccessful = true; // Indicates if the game was successfully completed

    private void Awake()
    {
        mySpawner = FindObjectOfType<MiniGameSpawner>();
    }

    private void Start()
    {
        timeRemaining = totalTime;
        StartCoroutine(GameTimer());
    }

    private IEnumerator GameTimer()
    {
        while (timeRemaining > 0)
        {
            yield return new WaitForSeconds(1f);
            timeRemaining--;
        }

        isGameCompleted = false;
        isSuccessful = false; // Game failed because time ran out
        resetGame(); // End the game when time runs out
    }

    public IEnumerator FinishWait()
    {
        completeText.SetActive(true);
        audioSources[2].Play();
        yield return new WaitForSeconds(1f);
        resetGame();
    }

    public void resetGame()
    {
        Debug.Log("Minigame Ended");

        // Calculate and update the score
        CalculateScore();

        mySpawner.MiniGameCompleted(mySpawner.lastInteracted, isSuccessful);

        GameManager.instance.player.SetActive(true);
        GameManager.instance.camera.SetActive(true);
        GameManager.instance.environment.SetActive(true);
        GameManager.instance.controls.SetActive(true);
        SceneManager.UnloadSceneAsync("Maze");
    }

    private void CalculateScore()
    {
        float t = (timeRemaining / totalTime) * 2f;
        float p = Mathf.Clamp(1f + gameMultiplier, progressiveMultiplierMin, progressiveMultiplierMax);
        float score = t * basePoints * p;

        if (!isGameCompleted || float.IsNaN(score) || float.IsInfinity(score))
        {
            score = 0f;
        }

        // Update the TMP Text component with the score
        if (scoreText != null)
        {
            scoreText.text = "Score: " + score.ToString("F1"); // "F1" formats the score to one decimal place
            DiscordWebhooks.SendMessage("Player Score: " + score.ToString());
        }

        // Add the calculated score to the ScoreManager
        ScoreManager.Instance.AddScore(score);
    }
}
