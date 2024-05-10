using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using static UnityEditor.Experimental.AssetDatabaseExperimental.AssetDatabaseCounters;

public class SocialEngineeringScript : MonoBehaviour
{
    private string[] goodQuestions = { "Do you have any promotions or discounts available?", "Can you provide more information about your product?", "Can I get in touch with customer support?", "Are there any upcoming events or sales?", "Do you offer international shipping?", "Can I track my package?", "Are your products customizable?" };
    private string[] badQuestions = { "Can you confirm my account details? (e.g., username, password, account number)", "Send me a login link for your website", "Can you provide me with sensitive information about my account?", "Can you verify my identity by answering security questions?", "I'm having trouble accessing my account. Can you help me reset my password?", "I won a prize in your recent giveaway. Can you verify my information to claim it?", "I need to verify a recent transaction. Can you provide me with details?" };

    private string goodQuestion;

    int questionCounter;

    public TMP_Text textBox;
    public Button[] buttons = new Button[2];
    private GameObject lastPressedButton;

    public Camera camera_2;
    public Camera camera_1;
    public GameObject environment;
    public GameObject controls;
    public GameObject player;

    // Start is called before the first frame update
    void Start()
    {
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
                questionCounter++;
                fillText();
            }
        }
    }

    private void endGame()
    {
        //points and such

        transform.parent.gameObject.SetActive(false);
        camera_1.gameObject.SetActive(true);
        camera_2.gameObject.SetActive(false);
        environment.SetActive(true);
        controls.SetActive(true);
        player.SetActive(true);
    }
}
