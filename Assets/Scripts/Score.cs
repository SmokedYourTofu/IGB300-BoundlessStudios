using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Score : MonoBehaviour
{
    public TMP_Text scoreText; // Reference to the TMP Text component for score
    public TMP_Text multiplierText; // Reference to the TMP Text component for multiplier
    public float totalTime; // Total time for the game
    public float basePoints; // Base number of points
    public float progressiveMultiplierMin; // Minimum progressive multiplier
    public float progressiveMultiplierMax; // Maximum progressive multiplier
    public float timeRemaining; // Time remaining for this game
    public float microGameCounter = 1f;
    public bool isGameCompleted = true; // Indicates if the game was completed
    public float gamesCompelted = 0;

    private void Start()
    {
        CalculateScore();
        UpdateMultiplierDisplay();
    }

    private void CalculateScore()
    {
        float t = (timeRemaining / totalTime) * 2f;
        float p = Mathf.Clamp(1f + ScoreManager.Instance.GameMultiplier, progressiveMultiplierMin, progressiveMultiplierMax);
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
        ScoreManager.Instance.AddMicrogameCounter(microGameCounter);
    }

    private void UpdateMultiplierDisplay()
    {
        if (multiplierText != null)
        {
            multiplierText.text = "x" + ScoreManager.Instance.GameMultiplier.ToString("F1");
        }
    }

    private void Update()
    {
        UpdateMultiplierDisplay();
    }

    private void UpdateMicrogames()
    {
        gamesCompelted++;
        ScoreManager.Instance.AddMicrogameCounter(gamesCompelted);
    }
}
