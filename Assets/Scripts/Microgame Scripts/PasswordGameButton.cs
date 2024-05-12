using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PasswordGameButton : MonoBehaviour
{

    public GameObject passwordController;
    private GameObject buttonText;
    private PasswordMicrogame psMicrogame;
    private string truePassword;
    private TMP_Text passwordText;
    public AudioSource[] passwordAudio = new AudioSource[2];

    private void Awake()
    {
        buttonText = this.transform.GetChild(0).gameObject;
        psMicrogame = passwordController.GetComponent<PasswordMicrogame>();
        passwordText = buttonText.GetComponent<TMP_Text>();
    }

    public void onButtonHit()
    {
        truePassword = psMicrogame.realPassword;
        if (passwordText.text == truePassword)
        {
            passwordAudio[0].Play();  
            //do score and other stuff
            psMicrogame.resetGame();
        }
        else
        {
            Debug.Log("wrong passwword");
            passwordAudio[0].Play();
        }
    }

}
