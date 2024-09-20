using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DeveloperToolbox;

public class SocialEngineeringScript : MonoBehaviour
{
    private string[] goodQuestions = { "Do you have any promotions or discounts available?", "Can you provide more information about your product?", "Can I get in touch with customer support?", "Are there any upcoming events or sales?", "Do you offer international shipping?", "Can I track my package?", "Are your products customizable?" };
    private string[] badQuestions = { "I need your data, NOW", "Send me account details so I can log into your website", "To confirm your safety I need all the sensitive information relating to your account", "Could you give me a list of all the most common answers to your security questions?", "Could you provide me with a list of all passwords used for accounts?", "I need you to give me the adresses of all of your users", "Could you give me a list of all transactions from the last hour?" };

    private string goodQuestion;

    int questionCounter;

    public TMP_Text textBox;
    public Button[] buttons = new Button[2];
    private GameObject lastPressedButton;

    public AudioSource[] audioSources;
    public GameObject completeText;

    private MiniGameSpawner mySpawner;

    private bool gameDone = false;

    public GameObject sceneCam;
    private ScreenShake shaker;

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

    // Start is called before the first frame update
    void Start()
    {
        mySpawner = FindObjectOfType<MiniGameSpawner>();
        shaker = sceneCam.GetComponent<ScreenShake>();
        fillText();
        foreach (Button button in buttons)
        {
            button.onClick.AddListener(() => ButtonPressed(button.gameObject));
        }

        timeRemaining = totalTime;
        StartCoroutine(GameTimer());
    }

    private void Update()
    {
        if (questionCounter == 3 && !gameDone)
        {
            gameDone = true;
            questionCounter = 0;
            audioSources[2].Play();
            StartCoroutine(FinishWait());
        }
    }

    private IEnumerator FinishWait()
    {
        completeText.SetActive(true);
        audioSources[1].Play();
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

    private void fillText()
    {
        int goodOrBad = Random.Range(1, 3);

        if (goodOrBad == 1)
        {
            goodQuestion = "Yes";
        }
        else
        {
            goodQuestion = "No";
        }

        if (goodQuestion == "Yes")
        {
            int choice = Random.Range(0, 7);
            textBox.text = goodQuestions[choice];
        }
        else if (goodQuestion == "No")
        {
            int choice = Random.Range(0, 7);
            textBox.text = badQuestions[choice];
        }
    }

    private void ButtonPressed(GameObject buttonGameObject)
    {
        // Store the GameObject of the pressed button
        lastPressedButton = buttonGameObject;

        // You can add any additional logic here when a button is pressed

        for (int i = 0; i < 2; i++)
        {
            if (lastPressedButton == buttons[i].gameObject && goodQuestion == lastPressedButton.transform.name && !gameDone)
            {
                audioSources[1].Play();
                StartCoroutine(changeColour(Color.green));
                questionCounter++;
                //fillText();
            }
            else if (lastPressedButton == buttons[i].gameObject && goodQuestion != lastPressedButton.transform.name && !gameDone)
            {
                //make players do another task if they get the wrong task
                Debug.Log("Wrong Choice");
                audioSources[0].Play();
                shaker.AddShake(10f);
                StartCoroutine(changeColour(Color.red));
                if (questionCounter != 0)
                {
                    questionCounter--;
                }
                //fillText();
            }
        }
    }

    private void endGame()
    {
        // Calculate and update the score
        CalculateScore();

        // Reset game for next time
        Debug.Log("Game Ended");
        mySpawner.MiniGameCompleted(mySpawner.lastInteracted, isSuccessful);

        GameManager.instance.player.SetActive(true);
        GameManager.instance.camera.SetActive(true);
        GameManager.instance.environment.SetActive(true);
        GameManager.instance.controls.SetActive(true);
        SceneManager.UnloadSceneAsync("Social Engineering");
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

    private IEnumerator changeColour(Color colour)
    {
        textBox.color = colour;
        yield return new WaitForSeconds(0.5f);
        textBox.color = Color.black;
        fillText();
    }
}
