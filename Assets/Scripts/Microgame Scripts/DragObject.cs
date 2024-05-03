using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;

public class DragObject3D : MonoBehaviour
{
    private bool isDragging = false;
    private Vector3 touchOffset;
    private Vector3 startPos;
    private GameObject myParent;
    public Camera camera_2;
    public Camera camera_1;
    public GameObject environment;
    public GameObject controls;
    public GameObject player;
    private float holdDistance;

    private void Start()
    {
        startPos = transform.position;
        transform.parent.gameObject.SetActive(false);
        holdDistance = Vector3.Distance(transform.position, camera_2.transform.position);
    }

    private void Update()
    {
        Drag();
    }

    private void Drag()
    {
        // Check if there is any touch input
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    // Cast a ray from the touch position to detect objects
                    Ray ray = camera_2.ScreenPointToRay(touch.position);
                    RaycastHit hit;

                    // If the ray hits an object, check if it's this object
                    if (Physics.Raycast(ray, out hit))
                    {
                        if (hit.collider.gameObject == gameObject)
                        {
                            // Calculate the offset between touch point and object position
                            touchOffset = hit.point - transform.position;
                            isDragging = true;
                        }
                    }
                    break;

                case TouchPhase.Moved:
                    // If the object is being dragged, move it according to touch position
                    if (isDragging)
                    {
                        Vector3 newPosition = new Vector3(touch.position.x, touch.position.y, holdDistance);
                        newPosition = camera_2.ScreenToWorldPoint(newPosition);
                        //newPosition.z = holdDistance;
                        transform.position = newPosition;
                    }
                    break;

                case TouchPhase.Ended:
                    // End dragging
                    isDragging = false;
                    if (Vector3.Distance(startPos, transform.position) > 10f)
                    {
                        this.transform.position = startPos;
                        //do points and stuff

                        transform.parent.gameObject.SetActive(false);
                        camera_1.gameObject.SetActive(true);
                        camera_2.gameObject.SetActive(false);
                        environment.SetActive(true);
                        controls.SetActive(true);
                        player.SetActive(true);
                    }
                    break;
            }
        }
    }
}


