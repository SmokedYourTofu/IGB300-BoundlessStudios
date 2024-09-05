using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MicroTimer : MonoBehaviour
{
    public Image timerbar;
    public float time;
    private float num;

    // Update is called once per frame
    void Update()
    {
        num = Time.deltaTime / time;
        timerbar.fillAmount -= num;

        if (timerbar.fillAmount >= 1 )
        {
            Destroy(this.gameObject);
        }
    }
}
