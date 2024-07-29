using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopUpsButton : MonoBehaviour
{
    public GameObject controller;
    private PopupController popupController;
    public AudioSource funSound;

    public void buttonPress()
    {
        popupController = controller.GetComponent<PopupController>();

        funSound.Play();
        StartCoroutine(popupController.FinishWait());
    }
}
