using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.UIElements;

public class DoorOpen : MonoBehaviour
{
    [SerializeField] private int timeWait;

    [SerializeField] private float rotateTimer;
    [SerializeField] private GameObject timerGameObject;
    [SerializeField] private bool timerOn;
    [SerializeField] private bool isOpen;
    [SerializeField] private bool openDoor;
    // Start is called before the first frame update
    private void Start()
    {
        timerOn = timerGameObject.GetComponent<Timer>().TimerOn;
        timerOn = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (timerOn)
        {
            if (rotateTimer > 0)
            {
                rotateTimer -= Time.deltaTime;
            }
            else
            {
                openDoor = true;
                rotateTimer = timeWait;
            }
        }

        if (openDoor)
        {

            if (transform.rotation.y < 5f)
            {
                isOpen = false;
            }
            else if (transform.rotation.y > 155f)
            {
                isOpen = true;
            }

            if (isOpen == true)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0f, 0f, 0f), 0.7f * Time.deltaTime);
            }
            else
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0f, 160f, 0f), 0.7f * Time.deltaTime);
            }

            if (transform.rotation.y < 0.1f && isOpen == true)
            {
                openDoor = false;
            }
            else if (transform.rotation.y > 159.8f && isOpen == false)
            {
                openDoor = false;
            }

        }
    }

}
