using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class DoorOpen : MonoBehaviour
{
    [SerializeField] private int timeWait;

    [SerializeField] private float rotateTimer;
    [SerializeField] private GameObject timerGameObject;
    [SerializeField] private bool timerOn;
    [SerializeField] private bool isOpen;
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
                if (isOpen == true)
                {
                    transform.rotation = Quaternion.Euler(0, 0, 0);
                    isOpen = false;
                }
                else
                {
                    transform.rotation = Quaternion.Euler(0, 160, 0);
                    isOpen = true;
                }
                rotateTimer = timeWait;
            }
        }
    }

}
