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
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.gameObject == gameObject)
                {
                    touchOffset = hit.point - transform.position;
                    isDragging = true;
                }
            }
        }
        else if (Input.GetMouseButton(0))
        {
            if (isDragging)
            {
                Vector3 newPosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, holdDistance);
                newPosition = Camera.main.ScreenToWorldPoint(newPosition);
                transform.position = newPosition;
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
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
