using System.Collections;
using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }

    public float TotalScore { get; private set; }
    public float GameMultiplier { get; private set; } = 1.0f; // Start with a multiplier of 1.0

    public Animator scoreAnimator;
    public Animator multAnimator;
    public TMP_Text scoreUpdate;
    public TMP_Text multUpdate;

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
    }

    public void AddScore(float score)
    {
        TotalScore += score;
        StartCoroutine(updateScore(score));
        Debug.Log("Total Score: " + TotalScore);
    }

    public void ResetScore()
    {
        TotalScore = 0f;
        GameMultiplier = 1.0f; // Reset multiplier to 1.0
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
