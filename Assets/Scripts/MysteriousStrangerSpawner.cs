using UnityEngine;
using System.Collections;

public class MysteriousStrangerSpawner : MonoBehaviour
{
    public GameObject mysteriousStrangerPrefab;  // Assign your enemy prefab in the Inspector
    public GameObject smokeBombPrefab;           // Assign your smoke bomb effect prefab
    public Transform[] spawnPoint;               // Assign the spawn location
    public Transform printerLocation;            // Assign the printer's location
    public GameObject printerObject;             // Reference to the printer object in the scene
    public GameObject indicatorPrefab;           // Reference to the indicator prefab
    public int scoreOnDestroy = 250;             // Score gained when the player destroys the mysterious stranger

    public float waitTimeAtPrinter = 0.5f;       // Time the enemy waits at the printer before despawning

    private GameObject spawnedEnemy;
    private GameObject spawnedIndicator;

    void Start()
    {
        // Start spawning the enemy with a delay
        InvokeRepeating("SpawnMysteriousStranger", 5f, 30f); // Spawn after 5 seconds, then every 30 seconds
    }

    void SpawnMysteriousStranger()
    {
        // Instantiate smoke bomb effect
        Instantiate(smokeBombPrefab, spawnPoint[Random.Range(0, spawnPoint.Length - 1)].position, Quaternion.identity);

        // Instantiate the enemy
        spawnedEnemy = Instantiate(mysteriousStrangerPrefab, spawnPoint[Random.Range(0, spawnPoint.Length - 1)].position, Quaternion.identity);

        // Enable collision detection on the mysterious stranger
        Collider collider = spawnedEnemy.GetComponent<Collider>();
        if (collider == null)
        {
            Debug.LogError("No collider found on the mysterious stranger prefab.");
        }

        // Get the enemy's movement script and set the target location
        EnemyMovement enemyMovement = spawnedEnemy.GetComponent<EnemyMovement>();
        if (enemyMovement != null)
        {
            enemyMovement.SetTarget(printerLocation);
        }

        // Disable the printer components while the enemy is active
        TogglePrinterComponents(false);

        // Spawn the indicator above the printer
        SpawnIndicator();
    }

    private void TogglePrinterComponents(bool isActive)
    {
        if (printerObject != null)
        {
            // Toggle the NewInteract component
            NewInteract interactScript = printerObject.GetComponent<NewInteract>();
            if (interactScript != null)
            {
                interactScript.enabled = isActive;

                // Activate or deactivate the microgame indicator
                ToggleMicrogameIndicator(isActive, interactScript);
            }

            // Toggle the Outline component
            Outline outlineScript = printerObject.GetComponent<Outline>();
            if (outlineScript != null)
            {
                outlineScript.enabled = isActive;
            }

            // Log error if components are missing
            if (interactScript == null || outlineScript == null)
            {
                Debug.LogError("NewInteract or Outline script not found on printer object.");
            }
        }
    }

    private void ToggleMicrogameIndicator(bool isActive, NewInteract interactScript)
    {
        if (isActive)
        {
            if (interactScript.indicator != null)
            {
                // Activate the microgame indicator when the printer becomes interactive
                interactScript.indicator.SetActive(true);
            }
        }
        else
        {
            if (interactScript.indicator != null)
            {
                // Deactivate the microgame indicator when the printer is not interactive
                interactScript.indicator.SetActive(false);
            }
        }
    }

    private void SpawnIndicator()
    {
        if (printerObject != null && indicatorPrefab != null)
        {
            Renderer renderer = printerObject.GetComponent<Renderer>();
            if (renderer != null)
            {
                // Calculate the position above the printer
                Vector3 topOfMesh = renderer.bounds.max;
                Vector3 indicatorPosition = new Vector3(topOfMesh.x, topOfMesh.y + 2, topOfMesh.z); // Adjust height as needed

                // Instantiate the indicator
                spawnedIndicator = Instantiate(indicatorPrefab, indicatorPosition, Quaternion.identity);

                // Maintain the original size of the indicator
                spawnedIndicator.transform.localScale = indicatorPrefab.transform.localScale;
            }
            else
            {
                Debug.LogError("Renderer not found on printer object.");
            }
        }
    }

    public void OnEnemyReachedPrinter()
    {
        StartCoroutine(DespawnAfterDelay());
    }

    private IEnumerator DespawnAfterDelay()
    {
        // Wait for the specified time
        yield return new WaitForSeconds(waitTimeAtPrinter);

        // Check if the enemy is still present before doing anything
        if (spawnedEnemy != null)
        {
            // Instantiate another smoke bomb before despawning
            Instantiate(smokeBombPrefab, spawnedEnemy.transform.position, Quaternion.identity);

            // Destroy the enemy
            Destroy(spawnedEnemy);
        }

        // Destroy the indicator
        if (spawnedIndicator != null)
        {
            Destroy(spawnedIndicator);
        }

        // Activate the printer components and the microgame indicator after the enemy despawns
        TogglePrinterComponents(true);
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Check if the player collided with the mysterious stranger
        if (collision.gameObject.CompareTag("Player") && spawnedEnemy != null)
        {
            // Player collided with mysterious stranger, destroy the stranger
            Destroy(spawnedEnemy);

            // Add 250 score to the player using the ScoreManager
            ScoreManager.Instance.AddScore(scoreOnDestroy);

            // Instantiate a smoke bomb at the enemy's position upon destruction
            Instantiate(smokeBombPrefab, spawnedEnemy.transform.position, Quaternion.identity);

            // Destroy the indicator
            if (spawnedIndicator != null)
            {
                Destroy(spawnedIndicator);
            }

            // Activate the printer components and the microgame indicator after the enemy is destroyed
            TogglePrinterComponents(true);
        }
    }
}