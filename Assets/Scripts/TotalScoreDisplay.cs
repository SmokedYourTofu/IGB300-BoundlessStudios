using UnityEngine;
using TMPro;

public class TotalScoreDisplay : MonoBehaviour
{
    public TMP_Text totalScoreText; // Reference to the TMP Text component
    public TMP_Text totalMicrogamesText;
    public TMP_Text highScoreText;
    public TMP_Text gradeText;

    private void Update()
    {
        UpdateTotalScore();
        UpdateTotalCompleted();
        UpdateHighScore();
        grade();
    }

    private void OnEnable()
    {
        UpdateTotalScore();
        UpdateTotalCompleted();
        UpdateHighScore();
        grade();
    }

    public void UpdateTotalScore()
    {
        if (totalScoreText != null && ScoreManager.Instance != null)
        {
            totalScoreText.text = "Score: " + ScoreManager.Instance.TotalScore.ToString("F1");
        }
    }

    public void UpdateTotalCompleted()
    {
        if (totalMicrogamesText != null && ScoreManager.Instance != null)
        {
            totalMicrogamesText.text = "Microgames completed: " + ScoreManager.Instance.TotalMicrogames.ToString("F1");
        }
    }

    public void UpdateHighScore()
    {
        if (highScoreText != null && ScoreManager.Instance != null)
        {
            highScoreText.text = "High Score: " + ScoreManager.Instance.personalBest.ToString("F1");
        }
    }

    public void grade()
    {
        if(gradeText != null && ScoreManager.Instance != null)
        {
            float score = ScoreManager.Instance.TotalScore;
            switch (true)
            {
                case bool _ when score > 2000:
                    gradeText.text = "A";
                    ScoreManager.Instance.level2 = true;
                    break;
                case bool _ when score > 1500:
                    gradeText.text = "B";
                    ScoreManager.Instance.level2 = true;
                    break;
                case bool _ when score > 1000:
                    gradeText.text = "C";
                    ScoreManager.Instance.level2 = true;
                    break;
                case bool _ when score > 500:
                    gradeText.text = "D";
                    break;
                case bool _ when score > 0:
                    gradeText.text = "E";
                    break;
                default:
                    gradeText.text = "F";
                    break;
            }
        }
    }
}
