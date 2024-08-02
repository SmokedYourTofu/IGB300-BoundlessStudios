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
#if !UNITY_ANDROID
        //this bool is changed in the handle script to detect if someone is touching it
        if (activateRotate)
        {
            //do some funny maths to make the crank rotate towards the last clicked/touched position
            lastRotation = newRotation;
            Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position);
            Vector3 vector = Input.mousePosition - screenPos;
            float angle = Mathf.Atan2(vector.y, vector.x) * Mathf.Rad2Deg;
            newRotation = Quaternion.AngleAxis(angle - startAngle, this.transform.forward);
            newRotation.y = 0;
            newRotation.eulerAngles = new Vector3(0, 0, newRotation.eulerAngles.z);
            this.transform.rotation = originalRotation * newRotation;
        }
#endif

#if UNITY_ANDROID
        //this bool is changed in the handle script to detect if someone is touching it
        if (activateRotate)
        {
            lastRotation = newRotation;

            if (Input.touchCount > 0)
            {
                //do some funny maths to make the crank rotate towards the last clicked/touched position
                Touch touch = Input.GetTouch(0);
                Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position);
                Vector3 touchVector = touch.position;
                Vector3 vector = touchVector - screenPos;
                float angle = Mathf.Atan2(vector.y, vector.x) * Mathf.Rad2Deg;
                newRotation = Quaternion.AngleAxis(angle - startAngle, this.transform.forward);
                newRotation.y = 0;
                newRotation.eulerAngles = new Vector3(0, 0, newRotation.eulerAngles.z);
                this.transform.rotation = originalRotation * newRotation;
            }
        }
#endif

        if (timerOn)
        {
            if (timerElapse < 0.2)
            {
                timerElapse += Time.deltaTime;
            }
            //every 0.2 seconds compare the last two rotations to see if the plaayer is cranking the right direction
            else
            {
                if (newRotation.eulerAngles.z  < lastRotation.eulerAngles.z && activateRotate)
                {
                    //if the player is rotating the right way play the hammer animation and track the progress thhrough the animation
                    hammerAnimator.speed = 0.2f;
                    crankProgess += 0.1f;
                    Debug.Log(crankProgess);

                }
                //if player isn't using the crank pause the animation
                else if (!activateRotate)
                {
                    hammerAnimator.speed = 0f;
                }
                timerElapse = 0;
            }
        }

        //once the hammer animation is complete, finish the game
        if (crankProgess > 1.8f)
        {
            hammerAnimator.speed = 0f;
            timerOn = false;

            hammerAnimator.speed = 1f;
            hammerAnimator.Play("HammerSmash");
        }

    }
}
