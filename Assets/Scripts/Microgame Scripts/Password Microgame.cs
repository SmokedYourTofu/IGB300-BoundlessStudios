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

    public TMP_Text passwordText;
    private GameObject[] buttons = new GameObject[6];

    private MiniGameSpawner mySpawner;

    private void Awake()
    {
        mySpawner = FindObjectOfType<MiniGameSpawner>();
        buttons = GameObject.FindGameObjectsWithTag("passwordButton");

        setupGame();
    }

    private void generateTruePassword()
    {
        //generate good pasword
        realPassword += words[UnityEngine.Random.Range(0, words.Length)];
        realPassword += words[UnityEngine.Random.Range(0, words.Length)];
        realPassword += number[UnityEngine.Random.Range(0, number.Length)];
        realPassword += specialChar[UnityEngine.Random.Range(0, specialChar.Length)];

        passwords[0] = realPassword;
    }

    private void generateFalsePasswords()
    {
        //generate password missing a number
        badPassword += words[UnityEngine.Random.Range(0, words.Length)];
        badPassword += words[UnityEngine.Random.Range(0, words.Length)];
        badPassword += specialChar[UnityEngine.Random.Range(0, specialChar.Length)];
        passwords[1] = badPassword;

        badPassword = null;

        //generate password missing a character
        badPassword += words[UnityEngine.Random.Range(0, words.Length)];
        badPassword += words[UnityEngine.Random.Range(0, words.Length)];
        badPassword += number[UnityEngine.Random.Range(0, number.Length)];
        passwords[2] = badPassword;

        badPassword = null;

        //generate password missing a word
        badPassword += words[UnityEngine.Random.Range(0, words.Length)];
        badPassword += number[UnityEngine.Random.Range(0, number.Length)];
        badPassword += specialChar[UnityEngine.Random.Range(0, specialChar.Length)];
        passwords[3] = badPassword;

        badPassword = null;

        //generate two bad passwords
        badPassword += words[UnityEngine.Random.Range(0, words.Length)];
        badPassword += words[UnityEngine.Random.Range(0, words.Length)];

        passwords[4] = badPassword;
        badPassword = null;

        badPassword += words[UnityEngine.Random.Range(0, words.Length)];
        badPassword += number[UnityEngine.Random.Range(0, number.Length)];
        passwords[5] = badPassword;

        badPassword = null;
    }

    public void resetGame()
    {
        Debug.Log("Minigame Ended");
        mySpawner.MiniGameCompleted(mySpawner.lastInteracted);

        GameManager.instance.player.SetActive(true);
        GameManager.instance.camera.SetActive(true);
        GameManager.instance.environment.SetActive(true);
        GameManager.instance.controls.SetActive(true);
        SceneManager.UnloadSceneAsync("Password Game");
    }

    public void setupGame()
    {
        realPassword = null;

        generateTruePassword();
        generateFalsePasswords();

        var rng = new System.Random();
        rng.Shuffle(passwords);
        rng.Shuffle(passwords);

        for (int i = 0; i < 6; i++)
        {
            TMP_Text buttonText = buttons[i].GetComponent<TMP_Text>();
            buttonText.text = passwords[i];
        }
    }
}

//shuffles array
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
