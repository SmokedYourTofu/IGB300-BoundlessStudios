using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }

    public float TotalScore { get; private set; }
    public float GameMultiplier { get; private set; } = 1.0f; // Start with a multiplier of 1.0

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
        Debug.Log("Game Multiplier: " + GameMultiplier);
    }
}
