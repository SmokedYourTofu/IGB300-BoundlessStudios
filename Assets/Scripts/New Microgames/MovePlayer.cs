using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MovePlayer : MonoBehaviour
{

    public GameObject text;
    private float startZ;
    public MazeContoller mazeContoller;

    private void Start()
    {
        startZ = transform.position.z;
    }

    void Update()
    {
        moveToMouse();
    }

#if !UNITY_ANDROID
    private void moveToMouse()
    {
        //when mouse is pressed/screen is touched send out a raycast
        if (Input.GetMouseButton(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                //move player to ray position
                Vector3 newPosition = hit.point;
                newPosition.z = startZ;
                transform.position = Vector3.MoveTowards(transform.position, newPosition, Time.deltaTime * 4);
            }
        }
        else
        {
            //when player isn't being dragged, stop them from moving
            this.GetComponent<Rigidbody>().velocity = Vector3.zero;
            transform.rotation = Quaternion.identity;
        }
    }
#endif

#if UNITY_ANDROID
    private void moveToMouse()
    {
        //when mouse is pressed/screen is touched send out a raycast
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began || touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary)
            {
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(touch.position);
                if (Physics.Raycast(ray, out hit))
                {
                    //move player to ray position
                    Vector3 newPosition = hit.point;
                    newPosition.z = startZ;
                    transform.position = Vector3.MoveTowards(transform.position, newPosition, Time.deltaTime * 4);
                }
            }
            else
            {
                //when player isn't being dragged, stop them from moving
                this.GetComponent<Rigidbody>().velocity = Vector3.zero;
                transform.rotation = Quaternion.identity;
            }
        }
    }
#endif

    private void OnCollisionEnter(Collision collision)
    {
        //end microgame when colliding with goal
        if (collision.gameObject.tag == "Goal")
        {
            StartCoroutine(mazeContoller.FinishWait());
        }
    }
}
