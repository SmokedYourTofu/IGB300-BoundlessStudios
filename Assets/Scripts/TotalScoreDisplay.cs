using UnityEngine;
using TMPro;

public class TotalScoreDisplay : MonoBehaviour
{
    public TMP_Text totalScoreText; // Reference to the TMP Text component

    private void Update()
    {
        UpdateTotalScore();
    }

    private void OnEnable()
    {
        UpdateTotalScore();
    }

    public void UpdateTotalScore()
    {
        if (totalScoreText != null && ScoreManager.Instance != null)
        {
            totalScoreText.text = "Score: " + ScoreManager.Instance.TotalScore.ToString("F1");
        }
    }
}
