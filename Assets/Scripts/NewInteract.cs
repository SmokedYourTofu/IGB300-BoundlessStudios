using UnityEngine;
using UnityEngine.SceneManagement;
using static MiniGameSpawner;

[RequireComponent(typeof(Outline))] // This ensures that the Outline component is added to the object
public class NewInteract : MonoBehaviour
{
    public float interactionRange = 3f; // Set the interaction range here
    private PlayerController playerController;
    private Outline outline; // Reference to the Outline component
    public string minigameName;
    private MiniGameSpawner miniGameSpawner;
    public GameObject indicator;
    public MiniGameTypes.MiniGameType miniGameType;

    private void Start()
    {
        miniGameSpawner = FindObjectOfType<MiniGameSpawner>();
        playerController = FindObjectOfType<PlayerController>();
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
        if (playerController == null || outline == null)
        {
            return;
        }

        // Check if the player clicked on this object
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

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
