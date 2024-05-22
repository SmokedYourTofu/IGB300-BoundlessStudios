using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragObjectNoEnd : MonoBehaviour
{
    private bool isDragging = false;
    private float holdDistance;

    public float rotationSpeed = 1f; // Speed of rotation
    private Vector2 startTouchPosition;
    private Vector2 currentTouchPosition;

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

                    isDragging = true;
                    startTouchPosition = touch.position;

                    break;

                case TouchPhase.Moved:
                    // If the object is being dragged, move it according to touch position
                    if (isDragging)
                    {
                        currentTouchPosition = touch.position;
                        Vector3 newPosition = new Vector3(touch.position.x, touch.position.y, holdDistance);
                        newPosition = Camera.main.ScreenToWorldPoint(newPosition);
                        //newPosition.z = holdDistance;
                        transform.position = newPosition;
                        RotateObject();
                    }
                    break;

                case TouchPhase.Stationary:
                    // If the object is being dragged, move it according to touch position
                    if (isDragging)
                    {
                        //startTouchPosition = touch.position;
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

    void RotateObject()
    {
        // Calculate the direction from the starting touch position to the current touch position
        Vector2 direction = currentTouchPosition - startTouchPosition;

        // Calculate the angle between the start and current touch positions
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Apply rotation to the GameObject
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward) * Quaternion.AngleAxis(rotationSpeed * Time.deltaTime, Vector3.up);

        // Update the start position for the next frame
        startTouchPosition = currentTouchPosition;
    }
}
