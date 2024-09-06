using UnityEngine;
using System.Collections;

public class MysteriousStrangerSpawner : MonoBehaviour
{
    public GameObject mysteriousStrangerPrefab;  // Assign your enemy prefab in the Inspector
    public GameObject smokeBombPrefab;           // Assign your smoke bomb effect prefab
    public Transform[] spawnPoint;                 // Assign the spawn location
    public Transform printerLocation;            // Assign the printer's location
    public GameObject printerObject;             // Reference to the printer object in the scene
    public GameObject indicatorPrefab;           // Reference to the indicator prefab

    public float waitTimeAtPrinter = 2f;         // Time the enemy waits at the printer before despawning

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
        Instantiate(smokeBombPrefab, spawnPoint[Random.Range(0,spawnPoint.Length - 1)].position, Quaternion.identity);

        // Instantiate the enemy
        spawnedEnemy = Instantiate(mysteriousStrangerPrefab, spawnPoint[Random.Range(0, spawnPoint.Length - 1)].position, Quaternion.identity);

        // Get the enemy's movement script and set the target location
        EnemyMovement enemyMovement = spawnedEnemy.GetComponent<EnemyMovement>();
        if (enemyMovement != null)
        {
            enemyMovement.SetTarget(printerLocation);
        }

        // Toggle the printer object to make it interactive
        TogglePrinterComponents(true);

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

        // Toggle off the printer components
        // TogglePrinterComponents(false);

        // Destroy the indicator
        if (spawnedIndicator != null)
        {
            Destroy(spawnedIndicator);
        }
    }

}