using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class Timer : MonoBehaviour
{

    public float TimeLeft;
    private bool TimerOn = false;

    private TMP_Text timerText;

    // Start is called before the first frame update
    void Start()
    {
        timerText = this.GetComponent<TMP_Text>();
        TimerOn = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (TimerOn)
        {
            if (TimeLeft > 0)
            {
                TimeLeft -= Time.deltaTime;
                updateTimer(TimeLeft);
            }
            else
            {
                Debug.Log("Time Up");
                TimeLeft = 0;
                TimerOn = false;
            }
        }
        
    }

    void updateTimer(float currentTime)
    {
        currentTime += 1;
        
        float minutes = Mathf.FloorToInt(currentTime / 60);
        float seconds = Mathf.FloorToInt(currentTime % 60);

        timerText.text = string.Format("{0:00} : {1:00}", minutes, seconds);
    }
}
