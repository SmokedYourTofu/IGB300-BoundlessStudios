using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PasswordMicrogame : MonoBehaviour
{

    private string characters = "abcdefghijklmnopqrstuvwxyz1234567890!@#$%^&*()[]{}./,";
    private string[] words = { "cows", "duck", "seen", "busy", "work", "name", "word", "safe" };
    private int length;
    private char requiredChar;
    public string[] passwords = new string[6];
    public string realPassword;
    private string badPassword;

    public TMP_Text passwordText;
    private GameObject[] buttons = new GameObject[6];

    private void Awake()
    {
        buttons = GameObject.FindGameObjectsWithTag("passwordButton");
        int length = Random.Range(8, 12);
        requiredChar = characters[Random.Range(0, 53)];

        generateTruePassword();
        generateFalsePasswords();

        var rng = new System.Random();
        rng.Shuffle(passwords);
        rng.Shuffle(passwords);

        passwordText.text = "Password must be at least " + length + " characters long, contain the character " + requiredChar + " and must not contain any recognisable words";

        for (int i = 0; i < 6; i++)
        {
            TMP_Text buttonText = buttons[i].GetComponent<TMP_Text>();
            buttonText.text = passwords[i];
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void generateTruePassword()
    {
        int requiredLoc = Random.Range(0, length);
        for (int i = 0; i < length; i++) 
        {
            if (i == requiredLoc)
            {
                realPassword = realPassword + requiredChar;
            }
            else
            {
                int a = Random.Range(0, 53);
                realPassword = realPassword + characters[a];
            }
        }
        passwords[0] = realPassword;
    }

    private void generateFalsePasswords()
    {
        //create password of incorrect length

        int lengthLess = Random.Range(0, 2);
        int newLength = length - lengthLess;
        int requiredLoc = Random.Range(0, lengthLess);
        for (int i = 0; i < lengthLess; i++)
        {
            if (i == requiredLoc)
            {
                badPassword = badPassword + requiredChar;
            }
            else
            {
                int a = Random.Range(0, 53);
                badPassword = badPassword + characters[a];
            }
        }
        passwords[1] = badPassword;
        badPassword = null;

        //create password with wrong character

        for (int i = 0; i < length; i++)
        {
            int a = Random.Range(0, 53);
            badPassword = badPassword + characters[a];
        }
        passwords[2] = badPassword;
        badPassword = null;

        //create password with recognisable word in it

        int randWord = Random.Range(0, 7);
        badPassword += words[randWord];
        requiredLoc = Random.Range(3, length);
        for (int i = 3; i < length; i++)
        {
            if (i == requiredLoc)
            {
                badPassword = badPassword + requiredChar;
            }
            else
            {
                int a = Random.Range(0, 53);
                badPassword = badPassword + characters[a];
            }
        }
        passwords[3] = badPassword;
        badPassword = null;

        //generate two more passwords with multiple issues

        randWord = Random.Range(0, 7);
        badPassword += words[randWord];
        requiredLoc = Random.Range(3, length);
        for (int i = 3; i < length; i++)
        {
            int a = Random.Range(0, 53);
            badPassword = badPassword + characters[a];
        }
        passwords[4] = badPassword;
        badPassword = null;

        for (int i = 0; i < length - 1; i++)
        {
            int a = Random.Range(0, 53);
            badPassword = badPassword + characters[a];
        }
        passwords[5] = badPassword;
        badPassword = null;
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
