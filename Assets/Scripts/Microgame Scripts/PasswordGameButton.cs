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

    private void Awake()
    {
        buttonText = this.transform.GetChild(0).gameObject;
        psMicrogame = passwordController.GetComponent<PasswordMicrogame>();
        truePassword = psMicrogame.realPassword;
        passwordText = buttonText.GetComponent<TMP_Text>();
    }

    public void onButtonHit()
    {
        if (passwordText.text == truePassword)
        {
            //do score and other stuff
        }
        else
        {
            Debug.Log("wrong passwword");
        }
    }

}
