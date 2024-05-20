using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
// using static UnityEditor.Experimental.AssetDatabaseExperimental.AssetDatabaseCounters;
using UnityEngine.SceneManagement;

public class SocialEngineeringScript : MonoBehaviour
{
    private string[] goodQuestions = { "Do you have any promotions or discounts available?", "Can you provide more information about your product?", "Can I get in touch with customer support?", "Are there any upcoming events or sales?", "Do you offer international shipping?", "Can I track my package?", "Are your products customizable?" };
    private string[] badQuestions = { "I need your data, NOW", "Send me account details so I can log into your website", "To confirm your safety I need all the sensitive information relating to your account", "Could you give me a list of all the most common answers to your security questions?", "Could you provide me with a list of all passwords used for accounts?", "I need you to give me the adresses of all of your users", "Could you give me a list of all transactions from the last hour?" };

    private string goodQuestion;

    int questionCounter;

    public TMP_Text textBox;
    public Button[] buttons = new Button[2];
    private GameObject lastPressedButton;

    private AudioSource[] audioSources;

    private MiniGameSpawner mySpawner;

    // Start is called before the first frame update
    void Start()
    {
        mySpawner = FindObjectOfType<MiniGameSpawner>();
        audioSources = GetComponents<AudioSource>();
        fillText();
        foreach (Button button in buttons)
        {
            button.onClick.AddListener(() => ButtonPressed(button.gameObject));
        }
    }

    private void Update()
    {
        if (questionCounter == 2)
        {
            endGame();
        }
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
            if (lastPressedButton == buttons[i].gameObject && goodQuestion == lastPressedButton.transform.name)
            {
                audioSources[0].Play();
                questionCounter++;
                fillText();
            }
            else if (lastPressedButton == buttons[i].gameObject && goodQuestion != lastPressedButton.transform.name)
            {
                //make players do another task if they get the wrong task
                Debug.Log("Wrong Choice");
                questionCounter--;
                fillText();
            }
        }
    }

    private void endGame()
    {
        audioSources[1].Play();
        Debug.Log("Game Over");
        //points and such

        mySpawner.MiniGameCompleted(mySpawner.lastInteracted);

        GameManager.instance.player.SetActive(true);
        GameManager.instance.camera.SetActive(true);
        GameManager.instance.environment.SetActive(true);
        GameManager.instance.controls.SetActive(true);
        SceneManager.UnloadSceneAsync("Social Engineering");
    }
}
