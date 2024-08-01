using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrankRotate : MonoBehaviour
{
    private float startAngle = 0;
    private Quaternion originalRotation;
    public bool activateRotate;

    private bool timerOn;
    private float timerElapse;
    private float crankProgess = 0;

    private Quaternion newRotation;
    private Quaternion lastRotation;

    public GameObject hammer;
    Animator hammerAnimator;

    public void Start()
    {
        originalRotation = this.transform.rotation;
        timerOn  = true;
        timerElapse = 0;

        hammerAnimator = hammer.GetComponent<Animator>();
        hammerAnimator.speed = 0;
        hammerAnimator.Play("HammerRotate");
    }
    void Update()
    {
        if (activateRotate)
        {
            lastRotation = newRotation;
            Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position);
            Vector3 vector = Input.mousePosition - screenPos;
            float angle = Mathf.Atan2(vector.y, vector.x) * Mathf.Rad2Deg;
            newRotation = Quaternion.AngleAxis(angle - startAngle, this.transform.forward);
            newRotation.y = 0; //see comment from above 
            newRotation.eulerAngles = new Vector3(0, 0, newRotation.eulerAngles.z);
            this.transform.rotation = originalRotation * newRotation;
        }

        if (timerOn)
        {
            if (timerElapse < 0.2)
            {
                timerElapse += Time.deltaTime;
            }
            else
            {
                if (newRotation.eulerAngles.z  < lastRotation.eulerAngles.z && activateRotate)
                {
                    hammerAnimator.speed = 0.2f;
                    crankProgess += 0.1f;
                    Debug.Log(crankProgess);

                }
                else if (!activateRotate)
                {
                    hammerAnimator.speed = 0f;
                }
                timerElapse = 0;
            }
        }

        if (crankProgess > 1.8f)
        {
            hammerAnimator.speed = 0f;
            timerOn = false;

            hammerAnimator.speed = 1f;
            hammerAnimator.Play("HammerSmash");
        }

    }
}
