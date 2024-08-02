using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrankHandle : MonoBehaviour
{

    public GameObject handlePar;
    private CrankRotate CR;

    private void Start()
    {
        CR = handlePar.GetComponent<CrankRotate>(); 
    }

#if !UNITY_ANDROID
    private void OnMouseDown()
    {
    //when mouse is clicked send out a ray
        Vector3 mousePosition = Input.mousePosition;
        Ray myRay = Camera.main.ScreenPointToRay(mousePosition);
        
        //if the ray hits the crank handle using the tag "border1" activate the rotate script
        if (Physics.Raycast(myRay, out RaycastHit hit))
        {
            // Use the hit variable to determine what was clicked on.
            if (hit.transform.tag == "border1")
            {
                CR.activateRotate = true;
            }
        }
    }

    private void OnMouseDrag()
    {
    //when mouse is held send out a ray
        Vector3 mousePosition = Input.mousePosition;
        Debug.Log(mousePosition);
        Ray myRay = Camera.main.ScreenPointToRay(mousePosition);

        //if the ray hits the crank handle using the tag "border1" activate the rotate script
        if (Physics.Raycast(myRay, out RaycastHit hit))
        {
            // Use the hit variable to determine what was clicked on.
            if (hit.transform.tag == "border1")
            {
                CR.activateRotate = true;
            }
            //if the ray is not hitting the handle, disable the script
            else
            {
                CR.activateRotate = false;
            }
        }
    }

    //if mouse isn't down disable the rotate script
    private void OnMouseUp()
    {
        CR.activateRotate = false;
    }
#endif

#if UNITY_ANDROID
    void Update()
    {
        if (Input.touchCount > 0)
        {
            //when a touch inpuut is made send a ray to touch position
            Touch touch = Input.GetTouch(0);
            Vector3 touchPosition = touch.position;
            Ray myRay = Camera.main.ScreenPointToRay(touchPosition);

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    //if the ray hits the crank handle using the tag "border1" activate the rotate script
                    if (Physics.Raycast(myRay, out RaycastHit hit))
                    {
                        // Use the hit variable to determine what was clicked on.
                        if (hit.transform.tag == "border1")
                        {
                            CR.activateRotate = true;
                        }
                    }
                    break;

                case TouchPhase.Moved:
                    //if the ray hits the crank handle using the tag "border1" activate the rotate script
                    if (Physics.Raycast(myRay, out RaycastHit hitTwo))
                    {
                        // Use the hit variable to determine what was clicked on.
                        if (hitTwo.transform.tag == "border1")
                        {
                            CR.activateRotate = true;
                        }
                        //if the ray is not hitting the handle, disable the script
                        else
                        {
                            CR.activateRotate = false;
                        }
                    }
                    break;

                case TouchPhase.Canceled:
                    //if the player is no longer touching, disable the script
                    CR.activateRotate = false;
                    break;
            }
        }
    }
#endif

}
