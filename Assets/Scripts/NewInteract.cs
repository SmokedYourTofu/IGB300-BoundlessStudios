using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using static MiniGameSpawner;

[RequireComponent(typeof(Outline))] // This ensures that the Outline component is added to the object
public class NewInteract : MonoBehaviour
{
    public float interactionRange = 3f; // Set the interaction range here
    private NewPlayerController playerController;
    private Outline outline; // Reference to the Outline component
    public string minigameName;
    private MiniGameSpawner miniGameSpawner;
    public GameObject indicator;
    public MiniGameTypes.MiniGameType miniGameType;

    private void Start()
    {
        miniGameSpawner = FindObjectOfType<MiniGameSpawner>();
        playerController = FindObjectOfType<NewPlayerController>();
        if (playerController == null)
        {
            Debug.LogError("PlayerController not found in the scene.");
        }

        outline = GetComponent<Outline>();
        if (outline == null)
        {
            Debug.LogError("Outline component not found on the object.");
        }
        else
        {
            outline.enabled = false; // Ensure the outline is initially disabled
        }
    }

    private void Update()
    {
        if (playerController == null)
        {
            playerController = FindObjectOfType<NewPlayerController>();
        }

        if (playerController == null || outline == null)
        {
            return;
        }

#if !UNITY_ANDROID
        // Check if the player clicked on this object
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            //assigning ray to something so editor is happy
            Ray ray = new Ray(transform.position, transform.forward);

            //checking if camera exists to cut down on errors
            if (Camera.main != null)
            {
                ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            }
            else
            {
                return;
            }

            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                if (hit.collider.gameObject == gameObject)
                {
                    // Get the distance between the player and the object
                    float distance = Vector3.Distance(transform.position, playerController.transform.position);

                    // Check if the player is within the interaction range
                    if (distance <= interactionRange)
                    {
                        Interact();
                    }
                    else
                    {
                        Debug.Log("Player is too far away to interact with this object.");
                    }
                }
            }
        }
#endif

#if UNITY_ANDROID
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            RaycastHit hit;
            // Assigning ray to something so editor is happy
            Ray ray = new Ray(transform.position, transform.forward);

            // Checking if camera exists to cut down on errors
            if (Camera.main != null)
            {
                ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
            }
            else
            {
                return;
            }

            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                if (hit.collider.gameObject == gameObject)
                {
                    // Get the distance between the player and the object
                    float distance = Vector3.Distance(transform.position, playerController.transform.position);

                    // Check if the player is within the interaction range
                    if (distance <= interactionRange)
                    {
                        Interact();
                    }
                    else
                    {
                        Debug.Log("Player is too far away to interact with this object.");
                    }
                }
            }
        }
#endif


        // Toggle outline based on distance
        float distToPlayer = Vector3.Distance(transform.position, playerController.transform.position);
        if (distToPlayer <= interactionRange)
        {
            outline.enabled = true;
        }
        else
        {
            outline.enabled = false;
        }
    }

    private void Interact()
    {
        // Implement your interaction logic here
        DiscordWebhooks.SendMessage("Minigame: " + minigameName + ", has been loaded");
        Debug.Log("Interacting with object!");

        
        SceneManager.LoadScene(minigameName, LoadSceneMode.Additive);
        miniGameSpawner.lastInteracted = this.gameObject;

        // Deactivate player, camera, environment, and controls
        GameManager.instance.player.SetActive(false);
        GameManager.instance.camera.SetActive(false);
        GameManager.instance.environment.SetActive(false);
        GameManager.instance.controls.SetActive(false);
    }

}