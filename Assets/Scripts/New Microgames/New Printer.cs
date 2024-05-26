using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.ProBuilder.Shapes;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NewPrinter : MonoBehaviour
{
    [SerializeField] private float barProgress = 0;
    public bool TimerOn = false;
    [SerializeField] private Animator myCable;
    private MiniGameSpawner mySpawner;

    public GameObject timer;
    private Slider timerbar;
    public AudioSource[] myAudioSource;
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

    // Start is called before the first frame update
    void Start()
    {
        timerbar = timer.GetComponent<Slider>();
        TimerOn = true;
        mySpawner = FindObjectOfType<MiniGameSpawner>();

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
        Endgame(); // End the game when time runs out
    }

    private IEnumerator Unplug()
    {
        completeText.SetActive(true);
        myCable.Play("PauseCable");
        yield return new WaitForSeconds(1f);
        Endgame();
    }

    // Update is called once per frame
    void Update()
    {
        if (TimerOn)
        {
            if (barProgress > 0)
            {
                barProgress -= Time.deltaTime;
                timerbar.value = barProgress;
            }
            else
            {
                barProgress = 0;
            }
        }

        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                myCable.Play("Cable Jiggle", 0, 0.0f);
                myAudioSource[1].pitch = barProgress / 10;
                myAudioSource[1].Play();
                if (barProgress < 6)
                {
                    barProgress += 1.5f;
                }
                else if (barProgress < 8)
                {
                    barProgress += 1f;
                }
                else if (barProgress < 10)
                {
                    barProgress += 0.8f;
                }
            }

            if (touch.phase == TouchPhase.Ended)
            {
                myCable.Play("Default", 0, 0.0f);
            }
        }

        if (barProgress >= 10)
        {
            myAudioSource[0].Play();
            StartCoroutine(Unplug());
            TimerOn = false;
        }
    }

    public void Endgame()
    {
        Debug.Log("Game Over");

        // Calculate and update the score
        CalculateScore();

        mySpawner.MiniGameCompleted(mySpawner.lastInteracted, isSuccessful);

        GameManager.instance.player.SetActive(true);
        GameManager.instance.camera.SetActive(true);
        GameManager.instance.environment.SetActive(true);
        GameManager.instance.controls.SetActive(true);

        UnloadSceneIfLoaded("Printer");
        UnloadSceneIfLoaded("Drive Smash");
    }

    private void UnloadSceneIfLoaded(string sceneName)
    {
        Scene scene = SceneManager.GetSceneByName(sceneName);
        if (scene.isLoaded)
        {
            SceneManager.UnloadSceneAsync(sceneName);
        }
        else
        {
            Debug.LogWarning($"Scene '{sceneName}' is not loaded and cannot be unloaded.");
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
