using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragObjectNoEnd : MonoBehaviour
{
    private bool isDragging = false;
    private float holdDistance;

    private void Start()
    {
        holdDistance = Vector3.Distance(transform.position, Camera.main.transform.position);
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
                    Ray ray = Camera.main.ScreenPointToRay(touch.position);
                    RaycastHit hit;

                    // If the ray hits an object, check if it's this object
                    if (Physics.Raycast(ray, out hit))
                    {
                        if (hit.collider.gameObject == gameObject)
                        {
                            // Calculate the offset between touch point and object position
                            isDragging = true;
                        }
                    }
                    break;

                case TouchPhase.Moved:
                    // If the object is being dragged, move it according to touch position
                    if (isDragging)
                    {
                        Vector3 newPosition = new Vector3(touch.position.x, touch.position.y, holdDistance);
                        newPosition = Camera.main.ScreenToWorldPoint(newPosition);
                        //newPosition.z = holdDistance;
                        transform.position = newPosition;
                    }
                    break;

                case TouchPhase.Ended:
                    // End dragging
                    isDragging = false;
                    break;
            }
        }
    }
}
