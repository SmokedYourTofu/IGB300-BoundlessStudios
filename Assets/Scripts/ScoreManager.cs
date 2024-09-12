using System.Collections;
using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }

    public float TotalScore { get; private set; }
    public float TotalMicrogames { get; private set; }
    public float GameMultiplier { get; private set; } = 1.0f; // Start with a multiplier of 1.0
    public float personalBest;

    public Animator scoreAnimator;
    public Animator multAnimator;
    public TMP_Text scoreUpdate;
    public TMP_Text multUpdate;
    public bool level2 = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        // Load the personal best score from PlayerPrefs if it exists
        if (PlayerPrefs.HasKey("PersonalBest"))
        {
            personalBest = PlayerPrefs.GetFloat("PersonalBest");
        }
        else
        {
            personalBest = 0; // Default if no score is found
        }

        // Display the personal best score at the start of the game (optional)
        Debug.Log("Personal Best: " + personalBest);
    }

    public void CheckForNewPersonalBest()
    {
        // Check if the current score is higher than the personal best
        if (TotalScore > personalBest)
        {
            personalBest = TotalScore;

            // Save the new personal best score in PlayerPrefs
            PlayerPrefs.SetFloat("PersonalBest", personalBest);
            PlayerPrefs.Save(); // Ensure it gets written to disk

            // Optionally display the new personal best
            Debug.Log("New Personal Best: " + personalBest);
        }
    }

    public void AddScore(float score)
    {
        TotalScore += score;
        StartCoroutine(updateScore(score));
        Debug.Log("Total Score: " + TotalScore);
    }

    public void AddMicrogameCounter(float gamesCompleted)
    {
        TotalMicrogames += gamesCompleted;
    }

    public void ResetScore()
    {
        TotalScore = 0f;
        GameMultiplier = 1.0f; // Reset multiplier to 1.0
        TotalMicrogames = 0f;
    }

    public void UpdateMultiplier(bool isSuccessful)
    {
        if (isSuccessful)
        {
            GameMultiplier = Mathf.Clamp(GameMultiplier + 0.05f, 0.2f, 2.0f);
        }
        else
        {
            GameMultiplier = Mathf.Clamp(GameMultiplier - 0.05f, 0.2f, 2.0f);
        }
        StartCoroutine(updateMult(isSuccessful));
        Debug.Log("Game Multiplier: " + GameMultiplier);
    }

    private IEnumerator updateScore(float score)
    {
        scoreUpdate.text = "+" + score.ToString("F1");
        yield return new WaitForSeconds(1f);
        scoreAnimator.Play("scoreUpdate");
        yield return new WaitForSeconds(1f);
        scoreUpdate.text = "";
        scoreAnimator.Play("Idle");
    }

    private IEnumerator updateMult(bool isSuccessful)
    {
        if (isSuccessful)
        {
            multUpdate.color = Color.green;
            multUpdate.text = "+";
        }
        else
        {
            multUpdate.color = Color.red;
            multUpdate.text = "-";
        }
        yield return new WaitForSeconds(1f);
        multAnimator.Play("multUpdate");
        yield return new WaitForSeconds(1f);
        multUpdate.text = "";
        multAnimator.Play("Idle");
    }
}
