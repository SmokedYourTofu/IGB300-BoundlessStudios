using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System.Collections;
using TMPro;

public class StickyNoteController : MonoBehaviour
{
    public Camera specificCamera;
    public List<GameObject> stickyNotePool;
    public List<GameObject> activeStickyNotes = new List<GameObject>();
    public AudioSource sound;
    public GameObject completeText;

    private MiniGameSpawner mySpawner;
    private TutorialScript tutorial;

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

    public void Start()
    {
        mySpawner = FindObjectOfType<MiniGameSpawner>();
        if (mySpawner == null)
        {
            tutorial = FindObjectOfType<TutorialScript>();
        }
        specificCamera = Camera.main;

        // Get a random number of sticky notes to activate (2-4)
        int stickyNoteCount = Random.Range(2, 5);

        // Shuffle the sticky note pool to randomize the selection
        Shuffle(stickyNotePool);

        // Activate the first 'stickyNoteCount' number of sticky notes
        for (int i = 0; i < stickyNoteCount; i++)
        {
            if (i < stickyNotePool.Count)
            {
                GameObject stickyNotePrefab = stickyNotePool[i];
                stickyNotePrefab.SetActive(true); // Activate the GameObject directly
                activeStickyNotes.Add(stickyNotePrefab);
            }
        }

        // Deactivate the remaining sticky notes
        for (int i = stickyNoteCount; i < stickyNotePool.Count; i++)
        {
            stickyNotePool[i].SetActive(false);
        }

        timeRemaining = totalTime;
        StartCoroutine(GameTimer());
    }

    public void Update()
    {
        // Check if there are no more active sticky notes
        if (activeStickyNotes.Count == 0)
        {
            sound.Play();
            StartCoroutine(FinishWait());
        }
    }

    private IEnumerator FinishWait()
    {
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

    public void Shuffle(List<GameObject> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            GameObject temp = list[i];
            int randomIndex = Random.Range(i, list.Count);
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }

    public void StickyNoteDestroyed(GameObject destroyedStickyNote)
    {
        // Remove the destroyed sticky note from the active list
        activeStickyNotes.Remove(destroyedStickyNote);

        // Check if there are no more active sticky notes
        if (activeStickyNotes.Count == 0)
        {
            StartCoroutine(FinishWait());
        }
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
        SceneManager.UnloadSceneAsync("Sticky Note");
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
