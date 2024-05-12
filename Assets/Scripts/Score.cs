using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Score : MonoBehaviour
{
    public TMP_Text scoreText; // Reference to the TMP Text component
    public float totalTime = 20f; // Total time for the game
    public float basePoints = 100f; // Base number of points
    public float progressiveMultiplierMin = 0.1f; // Minimum progressive multiplier
    public float progressiveMultiplierMax = 2f; // Maximum progressive multiplier
    public float gameMultiplier = 0.15f; // Game-specific multiplier
    public float timeRemaining = 5f; // Time remaining for this game
    public bool isGameCompleted = true; // Indicates if the game was completed

    // Start is called before the first frame update
    void Start()
    {
        CalculateScore();
    }

    private void CalculateScore()
    {
        float t = (timeRemaining / totalTime) * 2f;
        float p = Mathf.Clamp(1f + gameMultiplier, progressiveMultiplierMin, progressiveMultiplierMax);
        float score = t * basePoints * p;

        if (!isGameCompleted)
        {
            score = 0f;
        }

        // Update the TMP Text component with the score
        if (scoreText != null)
        {
            scoreText.text = "Score: " + score.ToString("F1"); // "F1" formats the score to one decimal place
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
