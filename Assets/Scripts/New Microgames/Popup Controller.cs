using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PopupController : MonoBehaviour
{
    public Camera specificCamera;
    public List<GameObject> PopupPool;
    public List<GameObject> activePopups = new List<GameObject>();
    public AudioSource[] sound;
    public GameObject completeText;

    private MiniGameSpawner mySpawner;
    private TutorialScript tutorial;

    public GameObject buttonImage;
    public Sprite endSprite;

    // Score parameters
    public TMP_Text scoreText; // Reference to the TMP Text component for displaying score
    public float totalTime = 30f; // Total time for the game (example value)
    public float basePoints = 100f; // Base number of points (example value)
    public float progressiveMultiplierMin = 0.1f; // Minimum progressive multiplier
    public float progressiveMultiplierMax = 2f; // Maximum progressive multiplier
    public float gameMultiplier = 0.15f; // Game-specific multiplier
    public float microGameCounter = 1f;
    private float timeRemaining; // Time remaining for this game
    private bool isGameCompleted = true; // Indicates if the game was completed
    private bool isSuccessful = true; // Indicates if the game was successfully completed

    public void Start()
    {
        mySpawner = FindObjectOfType<MiniGameSpawner>();
        if (mySpawner == null)
        {
            tutorial = FindObjectOfType<TutorialScript>();
        }
        specificCamera = Camera.main;

        // Get a random number of sticky notes to activate (2-4)
        int PopupCount = Random.Range(6, 9);

        // Activate the first 'stickyNoteCount' number of sticky notes
        for (int i = 0; i < PopupCount; i++)
        {
            if (i < PopupPool.Count)
            {
                GameObject PopUpPrefab = PopupPool[i];
                PopUpPrefab.SetActive(true); // Activate the GameObject directly
                activePopups.Add(PopUpPrefab);
            }
        }

        // Deactivate the remaining sticky notes
        for (int i = PopupCount; i < PopupPool.Count; i++)
        {
            PopupPool[i].SetActive(false);
        }

        timeRemaining = totalTime;
        StartCoroutine(GameTimer());
    }

    public void Update()
    {
        // Check if there are no more active sticky notes
        if (activePopups.Count == 0)
        {
            sound[0].Play();
            StartCoroutine(FinishWait());
        }
    }

    public IEnumerator FinishWait()
    {
        foreach (var popup in activePopups)
        {
            popup.GetComponent<ParticleSystem>().Play();
            popup.transform.GetChild(0).gameObject.SetActive(false);
        }
        buttonImage.GetComponent<SpriteRenderer>().sprite = endSprite;
        sound[2].Play();
        completeText.SetActive(true);
        yield return new WaitForSeconds(1f);
        endGame();
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
        endGame(); // End the game when time runs out
    }

    private void endGame()
    {
        // Calculate and update the score
        CalculateScore();

        // Reset game for next time
        Debug.Log("Game Ended");
        if (mySpawner != null)
        {
            mySpawner.MiniGameCompleted(mySpawner.lastInteracted, isSuccessful);
        }
        else
        {
            tutorial.MiniGameCompleted(tutorial.lastInteracted, isSuccessful);
        }

        GameManager.instance.player.SetActive(true);
        GameManager.instance.camera.SetActive(true);
        GameManager.instance.environment.SetActive(true);
        GameManager.instance.controls.SetActive(true);
        SceneManager.UnloadSceneAsync("Popups");
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
        ScoreManager.Instance.AddMicrogameCounter(microGameCounter);
    }
}
