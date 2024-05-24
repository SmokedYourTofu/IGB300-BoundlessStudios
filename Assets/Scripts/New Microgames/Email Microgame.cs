using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;

public class EmailMicrogame : MonoBehaviour
{
    private string[] goodSenders = { "John Smith", "David Lee", "Emily Rodriguez" };
    private string[] goodAddress = { "john.smith@company.com", "david.lee@company.com", "emily.rodriguez@company.com" };
    private string[] goodEmail = {
        "Dear Player,\r\n\r\nI would like to schedule a meeting to discuss the progress of Project X and address any challenges we may be facing. Could we arrange a convenient time for you to meet sometime this week?\r\n\r\nBest regards,\r\nJohn Smith",
        "Dear Player,\r\n\r\nAttached is the proposal document for Project 2 as discussed. Please review the document at your earliest convenience, and let me know if you have any questions or require further clarification.\r\n\r\nBest regards,\r\nDavid Lee\r\n",
        "Dear Player,\r\n\r\nI am writing to inquire about the service offered by your company. Could you please provide me with more information regarding its features, pricing, and any ongoing promotions?\r\n\r\nThank you and best regards,\r\nEmily Rodriguez\r\n"
    };
    private string[] goodSubject = { "Meeting Request for Project X Discussion", "Submission of Proposal for Project 2", "Inquiry Regarding the Service" };

    private string[] badAddress = { "john.smith@hackemail.com", "david.lee@hackemail.com", "emily.rodriguez@hackemail.com" };
    private string[] badEmail = {
        "Dear Player,\r\n\r\nI need your schedule for the week NOW!\r\n\r\nPlease send it soon otherwise your data will be GONE.",
        "Dear Player,\r\n\r\nDownload and run the program attached to this email.\r\n\r\nIt will install more ram on your computer and make it much safer (It's not bad at all, trust me).",
        "Dear Player,\r\n\r\nYour account is being hacked. I need you to give me your compaany username and password NOW!\r\n\r\nYour prompt response would be greatly appreciated (I promise I won't steal your account)."
    };
    private string[] badSubject = { "Meting Request for Progect X Diskusion", "Sabmision of Propsal for Projekt 2", "Incuiry Regrdin the Serfice" };

    public TMP_Text[] mainEmail = new TMP_Text[4];
    public TMP_Text[] fakeEmail = new TMP_Text[4];
    public Button[] buttons = new Button[4];
    private bool[] badSpots = { true, true, true, true };

    private GameObject lastPressedButton;

    private int choice;
    private int issues;
    private int newSpot;
    private int counter;

    public AudioSource[] audioSources;
    public GameObject completeText;

    private MiniGameSpawner mySpawner;

    // Score parameters
    public TMP_Text scoreText; // Reference to the TMP Text component for displaying score
    public float totalTime = 30f; // Total time for the game (example value)
    public float basePoints = 100f; // Base number of points (example value)
    public float progressiveMultiplierMin = 0.1f; // Minimum progressive multiplier
    public float progressiveMultiplierMax = 2f; // Maximum progressive multiplier
    public float gameMultiplier = 0.15f; // Game-specific multiplier
    private float timeRemaining; // Time remaining for this game
    private bool isGameCompleted = true; // Indicates if the game was completed

    // Start is called before the first frame update
    void Start()
    {
        mySpawner = FindObjectOfType<MiniGameSpawner>();
        choice = UnityEngine.Random.Range(0, 3);
        issues = UnityEngine.Random.Range(0, 3);
        foreach (Button button in buttons)
        {
            button.onClick.AddListener(() => ButtonPressed(button.gameObject));
        }

        ChooseEmail(choice);
        makeIssues(choice, issues);

        // Initialize the timer
        timeRemaining = totalTime;
        StartCoroutine(GameTimer());
    }

    private void Update()
    {
        if (counter - 1 == issues)
        {
            audioSources[2].Play();
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
        endGame(); // End the game when time runs out
    }

    private void ChooseEmail(int choice)
    {
        mainEmail[0].text = goodSubject[choice];
        mainEmail[1].text = goodSenders[choice];
        mainEmail[2].text = goodAddress[choice];
        mainEmail[3].text = goodEmail[choice];

        fakeEmail[0].text = goodSubject[choice];
        fakeEmail[1].text = goodSenders[choice];
        fakeEmail[2].text = goodAddress[choice];
        fakeEmail[3].text = goodEmail[choice];
    }

    private void makeIssues(int choice, int issues)
    {
        fakeEmail[0].text = goodSubject[choice];

        if (issues == 2)
        {
            fakeEmail[0].text = badSubject[choice];
            badSpots[0] = false;
            fakeEmail[2].text = badAddress[choice];
            badSpots[2] = false;
            fakeEmail[3].text = badEmail[choice];
            badSpots[3] = false;
        }
        else if (issues == 1)
        {
            int spot = UnityEngine.Random.Range(0, 3);
            if (spot == 2)
            {
                fakeEmail[3].text = badEmail[choice];
                badSpots[3] = false;
            }
            else if (spot == 1)
            {
                fakeEmail[2].text = badAddress[choice];
                badSpots[2] = false;
            }
            else
            {
                fakeEmail[0].text = badSubject[choice];
                badSpots[0] = false;
            }

            while (newSpot == spot)
            {
                newSpot = UnityEngine.Random.Range(0, 3);
            }

            if (newSpot == 2)
            {
                fakeEmail[3].text = badEmail[choice];
                badSpots[3] = false;
            }
            else if (newSpot == 1)
            {
                fakeEmail[2].text = badAddress[choice];
                badSpots[2] = false;
            }
            else
            {
                fakeEmail[0].text = badSubject[choice];
                badSpots[0] = false;
            }
        }
        else if (issues == 0)
        {
            int spot = UnityEngine.Random.Range(0, 3);
            if (spot == 2)
            {
                fakeEmail[3].text = badEmail[choice];
                badSpots[3] = false;
            }
            else if (spot == 1)
            {
                fakeEmail[2].text = badAddress[choice];
                badSpots[2] = false;
            }
            else
            {
                fakeEmail[0].text = badSubject[choice];
                badSpots[0] = false;
            }
        }
    }

    private void ButtonPressed(GameObject buttonGameObject)
    {
        // Store the GameObject of the pressed button
        lastPressedButton = buttonGameObject;

        for (int i = 0; i < 4; i++)
        {
            if (lastPressedButton == buttons[i].gameObject && badSpots[i] == false)
            {
                audioSources[1].Play();
                counter++;
                Debug.Log(counter + ", " + issues);
                buttons[i].GetComponent<Image>().color = Color.red;
                buttons[i].interactable = false;
            }
            else if (lastPressedButton == buttons[i].gameObject && badSpots[i] == true)
            {
                // Resets the game if they make the wrong choice, costing them time
                audioSources[0].Play();
                Debug.Log("Wrong Choice");
                choice = UnityEngine.Random.Range(0, 3);
                issues = UnityEngine.Random.Range(0, 3);
                ChooseEmail(choice);
                makeIssues(choice, issues);
                foreach (Button buttonObject in buttons)
                {
                    buttonObject.GetComponent<Image>().color = Color.white;
                    buttonObject.interactable = true;
                }
            }
        }
    }

    private void endGame()
    {

        // Calculate and update the score
        CalculateScore();

        // Reset game for next time
        Debug.Log("Game Ended");
        mySpawner.MiniGameCompleted(mySpawner.lastInteracted);

        GameManager.instance.player.SetActive(true);
        GameManager.instance.camera.SetActive(true);
        GameManager.instance.environment.SetActive(true);
        GameManager.instance.controls.SetActive(true);
        SceneManager.UnloadSceneAsync("Email");
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
