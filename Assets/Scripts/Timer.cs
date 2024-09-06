using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class Timer : MonoBehaviour
{
    public float TimeLeft;
    public bool TimerOn = false;
    public GameObject endgame;

    private TMP_Text timerText;

    public AudioSource music;

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
                endgame.SetActive(true);
                TimeLeft = 0;
                TimerOn = false;
                SendFinalScoreToDiscord();
                
            }

            if (TimeLeft <= 30)
            {
                music.pitch = 1.25f;
                var ratio = Mathf.Abs(Mathf.Sin(Time.time * 2f));
                timerText.color = Color.Lerp(Color.white, Color.red, ratio);
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

    void SendFinalScoreToDiscord()
    {
        float finalScore = ScoreManager.Instance.TotalScore;
        string message = $"Time is up! Final Player Score: {finalScore:F1}";
        DiscordWebhooks.SendMessage(message);
        //DiscordWebhooks.SendScreenshot();
        Debug.Log(message);
    }
}
