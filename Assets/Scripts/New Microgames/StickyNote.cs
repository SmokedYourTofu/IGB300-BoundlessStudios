using UnityEngine;

public class StickyNote : MonoBehaviour
{
    private bool isDragging = false;
    private Vector3 touchOffset;
    private Vector3 startPos;
    private float holdDistance;

    private AudioSource mySource;

    private void Start()
    {
        mySource = GetComponent<AudioSource>();
        startPos = transform.position;

        // Find the StickyNoteController instance in the scene
        StickyNoteController controller = FindObjectOfType<StickyNoteController>();
        if (controller != null && controller.specificCamera != null)
        {
            holdDistance = Vector3.Distance(transform.position, controller.specificCamera.transform.position);
        }
        else
        {
            Debug.LogError("StickyNoteController instance not found or specificCamera not set.");
        }
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
            Ray ray = Camera.main.ScreenPointToRay(touch.position);
            RaycastHit hit;

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    if (Physics.Raycast(ray, out hit))
                    {
                        if (hit.collider.gameObject == gameObject)
                        {
                            touchOffset = hit.point - transform.position;
                            isDragging = true;
                        }
                    }
                    break;

                case TouchPhase.Moved:
                case TouchPhase.Stationary:
                    if (isDragging)
                    {
                        Vector3 newPosition = new Vector3(touch.position.x, touch.position.y, holdDistance);
                        newPosition = Camera.main.ScreenToWorldPoint(newPosition) - touchOffset;
                        transform.position = newPosition;
                    }
                    break;

                case TouchPhase.Ended:
                case TouchPhase.Canceled:
                    isDragging = false;
                    break;
            }
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        // Check if the sticky note has collided with the 'bin' game object
        if (other.gameObject.CompareTag("Bin"))
        {
            Destroy(gameObject);
            // Remove the destroyed sticky note from the active list using the instance of StickyNoteController
            StickyNoteController instance = FindObjectOfType<StickyNoteController>();
            if (instance != null)
            {
                instance.activeStickyNotes.Remove(gameObject);
            }
            else
            {
                Debug.LogError("StickyNoteController instance not found.");
            }
        }
    }
}

