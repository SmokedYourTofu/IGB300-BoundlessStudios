using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnMicrogame : MonoBehaviour
{
    public GameObject Timer;
    private Timer timerScript;
    private float microTime = 1;

    public GameObject[] microObjects;
    private List<MeshRenderer> outlines = new List<MeshRenderer>();
    private int microChoice;

    // Start is called before the first frame update
    void Start()
    {
        timerScript = Timer.GetComponent<Timer>();
        for (int i = 0; i < microObjects.Length; i++)
        {
            outlines.Add(microObjects[i].GetComponent<MeshRenderer>());
            outlines[i].enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (timerScript.TimerOn) 
        {
            if (microTime < 10)
            {
                microTime += Time.deltaTime;
            }
            else
            {
                Debug.Log("Start MicroGame");
                ActivatePuzzle();
                microTime = 0;
            }
        }
    }

    void ActivatePuzzle()
    {
        while (timerScript.TimeLeft > 0)
        {
            // eventually turn this into a forloop? - alex
            microChoice = Random.Range(1, 6);
            if (microChoice == 1)
            {
                if (outlines[0].enabled == false)
                {
                    outlines[0].enabled = true;
                }
                else
                {
                    microChoice++;
                }
            }
            if (microChoice == 2)
            {
                if (outlines[1].enabled == false)
                {
                    outlines[1].enabled = true;
                }
                else
                {
                    microChoice++;
                }
            }
            if (microChoice == 3)
            {
                if (outlines[2].enabled == false)
                {
                    outlines[2].enabled = true;
                }
                else
                {
                    microChoice++;
                }
            }
            if (microChoice == 4)
            {
                if (outlines[3].enabled == false)
                {
                    outlines[3].enabled = true;
                }
                else
                {
                    microChoice++;
                }
            }
            if (microChoice == 5)
            {
                if (outlines[4].enabled == false)
                {
                    outlines[4].enabled = true;
                }
                else
                {
                    microChoice++;
                }
            }
            if (microChoice == 6)
            {
                if (outlines[5].enabled == false)
                {
                    outlines[5].enabled = true;
                }
                else
                {
                    microChoice++;
                }
            }
        }
    }
}
