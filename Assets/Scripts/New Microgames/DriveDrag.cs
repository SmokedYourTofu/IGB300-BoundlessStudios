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
#if UNITY_ANDROID
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                // Cast a ray from the touch position to detect objects
                Ray ray = Camera.main.ScreenPointToRay(touch.position);
                RaycastHit hit;

                // If the ray hits an object, check if it's this object
                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.collider.gameObject == gameObject)
                    {
                        // Start dragging
                        isDragging = true;
                    }
                }
            }
            else if ((touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary) && isDragging)
            {
                // If the object is being dragged, move it according to touch position
                Vector3 newPosition = new Vector3(touch.position.x, touch.position.y, holdDistance);
                newPosition = Camera.main.ScreenToWorldPoint(newPosition);
                transform.position = newPosition;
            }
            else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
            {
                // End dragging
                isDragging = false;
            }
        }
#endif

#if !UNITY_ANDROID
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                // Cast a ray from the touch position to detect objects
                Ray ray = Camera.main.ScreenPointToRay(touch.position);
                RaycastHit hit;

                // If the ray hits an object, check if it's this object
                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.collider.gameObject == gameObject)
                    {
                        // Start dragging
                        isDragging = true;
                    }
                }
            }
            else if ((touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary) && isDragging)
            {
                // If the object is being dragged, move it according to touch position
                Vector3 newPosition = new Vector3(touch.position.x, touch.position.y, holdDistance);
                newPosition = Camera.main.ScreenToWorldPoint(newPosition);
                transform.position = newPosition;
            }
            else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
            {
                // End dragging
                isDragging = false;
            }
        }
#endif
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
