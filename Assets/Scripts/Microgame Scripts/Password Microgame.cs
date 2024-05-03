using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PasswordMicrogame : MonoBehaviour
{

    private string characters = "abcdefghijklmnopqrstuvwxyz1234567890!@#$%^&*()[]{}./,";
    private string[] words = { "cow", "duck", "seen", "busy", "work", "name", "word", "safe" };
    private int length;
    private char requiredChar;
    public List<string> passwords = new List<string>();
    public string realPassword;

    private void Awake()
    {
        int length = Random.Range(8, 12);
        requiredChar = characters[Random.Range(0, 53)];
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
    }
}
