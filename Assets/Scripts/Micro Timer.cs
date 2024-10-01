using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MicroTimer : MonoBehaviour
{
    public Image timerbar;
    public float time;
    private float num;
    private SoundManager soundManager;
    public AudioClip endEffect;

    private void Start()
    {
        soundManager = FindObjectOfType<SoundManager>();
    }

    // Update is called once per frame
    void Update()
    {
        num = Time.deltaTime / time;
        timerbar.fillAmount -= num;

        if (num >= 1 )
        {
            soundManager.PlaySoundFXclip(endEffect, this.transform, 1.0f);
            Destroy(this.gameObject);
        }
    }
}
