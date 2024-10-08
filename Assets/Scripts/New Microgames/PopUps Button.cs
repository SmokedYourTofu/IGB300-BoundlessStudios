using DeveloperToolbox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopUpsButton : MonoBehaviour
{
    public GameObject controller;
    private PopupController popupController;
    public AudioSource funSound;

    public GameObject sceneCam;
    private ScreenShake shaker;

    private void Start()
    {
        shaker = sceneCam.GetComponent<ScreenShake>();
    }
    public void buttonPress()
    {
        //wwhhen the buttton is pressed, complette the game
        popupController = controller.GetComponent<PopupController>();
        shaker.AddShake(10f);
        funSound.Play();
        StartCoroutine(popupController.FinishWait());
    }
}
