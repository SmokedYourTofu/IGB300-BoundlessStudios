using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.SceneManagement;

public class PasswordMicrogame : MonoBehaviour
{
    private string number = "1234576890";
    private string specialChar = "!?@$.,':";
    private string[] words = { "Cows", "Duck", "Seen", "Busy", "Work", "Name", "Word", "Safe", "Happy", "Sad", "Silly", "Help", "Cool", "Man", "Woman", "Cheese" };
    public string[] passwords = new string[6];
    public string realPassword;
    private string badPassword;

    public AudioSource[] audioSources;
    public TMP_Text passwordText;
    private GameObject[] buttons = new GameObject[6];

    private MiniGameSpawner mySpawner;
    public GameObject completeText;

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

    private void Awake()
    {
        mySpawner = FindObjectOfType<MiniGameSpawner>();
        buttons = GameObject.FindGameObjectsWithTag("passwordButton");

        setupGame();
    }

    private void Start()
    {
        timeRemaining = totalTime;
        StartCoroutine(GameTimer());
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
        resetGame(); // End the game when time runs out
    }

    public IEnumerator FinishWait()
    {
        completeText.SetActive(true);
        audioSources[2].Play();
        yield return new WaitForSeconds(1f);
        resetGame();
    }

    private void generateTruePassword()
    {
        // Generate good password
        realPassword = words[UnityEngine.Random.Range(0, words.Length)];
        realPassword += words[UnityEngine.Random.Range(0, words.Length)];
        realPassword += number[UnityEngine.Random.Range(0, number.Length)];
        realPassword += specialChar[UnityEngine.Random.Range(0, specialChar.Length)];

        passwords[0] = realPassword;
    }

    private void generateFalsePasswords()
    {
        // Generate password missing a number
        badPassword = words[UnityEngine.Random.Range(0, words.Length)];
        badPassword += words[UnityEngine.Random.Range(0, words.Length)];
        badPassword += specialChar[UnityEngine.Random.Range(0, specialChar.Length)];
        passwords[1] = badPassword;

        badPassword = "";

        // Generate password missing a character
        badPassword = words[UnityEngine.Random.Range(0, words.Length)];
        badPassword += words[UnityEngine.Random.Range(0, words.Length)];
        badPassword += number[UnityEngine.Random.Range(0, number.Length)];
        passwords[2] = badPassword;

        badPassword = "";

        // Generate password missing a word
        badPassword = words[UnityEngine.Random.Range(0, words.Length)];
        badPassword += number[UnityEngine.Random.Range(0, number.Length)];
        badPassword += specialChar[UnityEngine.Random.Range(0, specialChar.Length)];
        passwords[3] = badPassword;

        badPassword = "";

        // Generate two bad passwords
        badPassword = words[UnityEngine.Random.Range(0, words.Length)];
        badPassword += words[UnityEngine.Random.Range(0, words.Length)];

        passwords[4] = badPassword;
        badPassword = "";

        badPassword = words[UnityEngine.Random.Range(0, words.Length)];
        badPassword += number[UnityEngine.Random.Range(0, number.Length)];
        passwords[5] = badPassword;

        badPassword = "";
    }

    public void resetGame()
    {
        Debug.Log("Minigame Ended");

        // Calculate and update the score
        CalculateScore();

        mySpawner.MiniGameCompleted(mySpawner.lastInteracted, isSuccessful);

        GameManager.instance.player.SetActive(true);
        GameManager.instance.camera.SetActive(true);
        GameManager.instance.environment.SetActive(true);
        GameManager.instance.controls.SetActive(true);
        SceneManager.UnloadSceneAsync("Password");
    }

    public void setupGame()
    {
        realPassword = "";

        generateTruePassword();
        generateFalsePasswords();

        var rng = new System.Random();
        rng.Shuffle(passwords);
        rng.Shuffle(passwords);

        for (int i = 0; i < 6; i++)
        {
            TMP_Text buttonText = buttons[i].GetComponentInChildren<TMP_Text>();
            buttonText.text = passwords[i];
        }
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

// Shuffles array
static class RandomExtensions
{
    public static void Shuffle<T>(this System.Random rng, T[] array)
    {
        int n = array.Length;
        while (n > 1)
        {
            int k = rng.Next(n--);
            T temp = array[n];
            array[n] = array[k];
            array[k] = temp;
        }
    }
}
