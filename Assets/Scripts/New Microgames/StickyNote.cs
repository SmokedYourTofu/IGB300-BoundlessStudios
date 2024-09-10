using UnityEngine;
using TMPro;

public class StickyNote : MonoBehaviour
{
    private bool isDragging = false;
    private Vector3 touchOffset;
    private Vector3 startPos;
    private float holdDistance;

    public TMP_Text thisText;
    private string number = "1234576890";
    private string specialChar = "!?@$#";
    private string[] words = { "Cows", "Duck", "Seen", "Busy", "Work", "Name", "Word", "Safe", "Happy", "Sad", "Silly", "Help", "Cool", "Man", "Woman", "Cheese" };
    public string realPassword;

    private AudioSource mySource;
    public AudioSource[] paperSounds;

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

        realPassword = words[UnityEngine.Random.Range(0, words.Length)];
        realPassword += words[UnityEngine.Random.Range(0, words.Length)];
        realPassword += number[UnityEngine.Random.Range(0, number.Length)];
        realPassword += specialChar[UnityEngine.Random.Range(0, specialChar.Length)];

        thisText.text = "Password: " + realPassword;
    }

    private void Update()
    {
        Drag();
    }

#if UNITY_ANDROID
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
#endif

#if !UNITY_ANDROID
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
        else if (Input.GetMouseButton(0) && isDragging)
        {
            Vector3 newPosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, holdDistance);
            newPosition = Camera.main.ScreenToWorldPoint(newPosition) - touchOffset;
            transform.position = newPosition;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
        }
    }
#endif

    public void OnTriggerEnter(Collider other)
    {
        // Check if the sticky note has collided with the 'bin' game object
        if (other.gameObject.CompareTag("Bin"))
        {
            Destroy(gameObject);
            paperSounds[Random.Range(0, paperSounds.Length)].Play();
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

