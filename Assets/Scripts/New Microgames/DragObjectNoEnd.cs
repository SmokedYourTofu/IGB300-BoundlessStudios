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

    public float mouseSpeed;

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
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                isDragging = true;
                startTouchPosition = touch.position;
            }
            else if ((touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary) && isDragging)
            {
                currentTouchPosition = touch.position;
                Vector3 newPosition = new Vector3(currentTouchPosition.x, currentTouchPosition.y, holdDistance);
                newPosition = Camera.main.ScreenToWorldPoint(newPosition);
                transform.position = newPosition;
                RotateObject();
                mouseSpeed = Time.deltaTime;
                Debug.Log(mouseSpeed);
            }
            else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
            {
                isDragging = false;
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


