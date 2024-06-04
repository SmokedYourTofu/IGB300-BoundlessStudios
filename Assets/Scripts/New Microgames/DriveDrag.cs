using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DriveDrag : MonoBehaviour
{
    private bool isDragging = false;
    private float holdDistance;
    public DriveDragControl dragControl;

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
            // Cast a ray from the mouse position to detect objects
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            // If the ray hits an object, check if it's this object
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.gameObject == gameObject)
                {
                    // Calculate the offset between mouse point and object position
                    isDragging = true;
                }
            }
        }
        else if (Input.GetMouseButton(0) && isDragging)
        {
            // If the object is being dragged, move it according to mouse position
            Vector3 newPosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, holdDistance);
            newPosition = Camera.main.ScreenToWorldPoint(newPosition);
            transform.position = newPosition;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            // End dragging
            isDragging = false;
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Bin"))
        {
            Destroy(gameObject);
            dragControl.Endgame();
        }
    }
}