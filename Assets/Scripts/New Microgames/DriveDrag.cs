using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DriveDrag : MonoBehaviour
{
    private bool isDragging = false;
    private float holdDistance;
    public GameObject otherDrive;
    public DragObjectNoEnd hammerScript;

    private Vector3 startPos;

    private bool notTouched = true;
    public AudioSource[] driveSounds;
    public TMP_Text instructions;

    private void Start()
    {
        holdDistance = Vector3.Distance(transform.position, Camera.main.transform.position);
        startPos = transform.position;
        hammerScript.enabled = false;
        otherDrive.gameObject.SetActive(false);
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
                        if (notTouched)
                        {
                            notTouched = false;
                            driveSounds[0].Play();
                        }
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
                transform.position = startPos;
            }
        }
#endif

#if !UNITY_ANDROID
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
                    if (notTouched)
                    {
                        notTouched = false;
                        driveSounds[0].Play();
                    }
                    // Start dragging
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
            transform.position = startPos;
        }
#endif
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Bin"))
        {
            driveSounds[1].Play();
            Destroy(gameObject);
            otherDrive.gameObject.SetActive(true);
            hammerScript.enabled = true;
            instructions.text = "SWING FAST!";
        }
    }
}
