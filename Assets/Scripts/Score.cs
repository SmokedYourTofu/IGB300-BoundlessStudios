using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;

public class Score : MonoBehaviour
{
    public TMP_Text scoreText; // Reference to the TMP Text component
    public float totalTime; // Total time for the game
    public float basePoints; // Base number of points
    public float progressiveMultiplierMin; // Minimum progressive multiplier
    public float progressiveMultiplierMax; // Maximum progressive multiplier
    public float gameMultiplier; // Game-specific multiplier
    public float timeRemaining; // Time remaining for this game
    public bool isGameCompleted = true; // Indicates if the game was completed

    private void Start()
    {
        CalculateScore();
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
        }

        // Add the calculated score to the ScoreManager
        ScoreManager.Instance.AddScore(score);
    }

    private void Update()
    {
    }
}
