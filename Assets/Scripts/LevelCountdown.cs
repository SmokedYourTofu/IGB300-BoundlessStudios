using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelCountdown : MonoBehaviour
{
    private TMP_Text countDownText;
    public GameObject countDownObj;
    public GameObject gameManager;
    public GameObject roomba;
    private movingBumper roombaScript;
    private MiniGameSpawner spawnerScript;
    public GameObject mysteriousStranger;
    public GameObject Timer;
    public GameObject controls;

    public AudioSource beepEffect;
    public AudioSource beepEnd;

    // Start is called before the first frame update
    void Start()
    {
        //initialise any script variables
        roombaScript = roomba.GetComponent<movingBumper>();
        countDownText = countDownObj.GetComponent<TMP_Text>();
        spawnerScript = gameManager.GetComponent<MiniGameSpawner>();

        //make sure all important objects are disbale before level start
        Timer.SetActive(false);
        spawnerScript.enabled = false;
        roombaScript.enabled = false;

        //check for if the level contains mysterious stranger
        if (mysteriousStranger != null)
        {
            mysteriousStranger.SetActive(false);
        }

        controls.SetActive(false);

        //begin countdowwn
        StartCoroutine(countDowm());
    }

    private IEnumerator countDowm()
    {
        //change text every second for countdown
        countDownText.text = "3";
        beepEffect.Play();
        yield return new WaitForSeconds(1);
        countDownText.text = "2";
        beepEffect.Play();
        yield return new WaitForSeconds(1);
        countDownText.text = "1";
        beepEffect.Play();
        yield return new WaitForSeconds(1);
        countDownText.text = "GO!";
        beepEnd.Play();

        //re-enabled everything when game starts
        Timer.SetActive(true);
        spawnerScript.enabled = true;
        roombaScript.enabled = true;
        if (mysteriousStranger != null)
        {
            mysteriousStranger.SetActive(true);
        }
        controls.SetActive(false);
        controls.SetActive(true);

        //allow a second before countdown disables
        yield return new WaitForSeconds(1);

        countDownObj.SetActive(false);


    }
}
