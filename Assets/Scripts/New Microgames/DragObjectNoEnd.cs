using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragObjectNoEnd : MonoBehaviour
{
    private bool isDragging = false;
    private float holdDistance;

    public float rotationSpeed = 1f; // Speed of rotation
    private Vector2 startMousePosition;
    private Vector2 currentMousePosition;

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
        if (Input.GetMouseButtonDown(0))
        {
            isDragging = true;
            startMousePosition = Input.mousePosition;
        }
        else if (Input.GetMouseButton(0) && isDragging)
        {
            currentMousePosition = Input.mousePosition;
            Vector3 newPosition = new Vector3(currentMousePosition.x, currentMousePosition.y, holdDistance);
            newPosition = Camera.main.ScreenToWorldPoint(newPosition);
            transform.position = newPosition;
            RotateObject();
            mouseSpeed = Time.deltaTime;
            Debug.Log(mouseSpeed);
        }
        else if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
        }
    }

    void RotateObject()
    {
        // Calculate the direction from the starting mouse position to the current mouse position
        Vector2 direction = currentMousePosition - startMousePosition;

        // Calculate the angle between the start and current mouse positions
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Apply rotation to the GameObject
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward) * Quaternion.AngleAxis(rotationSpeed * Time.deltaTime, Vector3.up);

        // Update the start position for the next frame
        startMousePosition = currentMousePosition;
    }
}

