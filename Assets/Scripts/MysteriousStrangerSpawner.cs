using UnityEngine;
using System.Collections;

public class MysteriousStrangerSpawner : MonoBehaviour
{
    public GameObject mysteriousStrangerPrefab;  // Assign your enemy prefab in the Inspector
    public GameObject smokeBombPrefab;         // Assign your smoke bomb effect prefab
    public Transform spawnPoint;                // Assign the spawn location
    public Transform printerLocation;           // Assign the printer's location

    // Time the enemy waits at the printer before despawning
    public float waitTimeAtPrinter = 2f;

    private GameObject spawnedEnemy;

    void Start()
    {
        // Start spawning the enemy with a delay
        InvokeRepeating("SpawnMysteriousStranger", 5f, 30f); // Spawn after 5 seconds, then every 30
    }

    void SpawnMysteriousStranger()
    {
        // Instantiate smoke bomb effect
        Instantiate(smokeBombPrefab, spawnPoint.position, Quaternion.identity);

        // Instantiate the enemy
        spawnedEnemy = Instantiate(mysteriousStrangerPrefab, spawnPoint.position, Quaternion.identity);

        // Get the enemy's movement script (you'll need to create this)
        EnemyMovement enemyMovement = spawnedEnemy.GetComponent<EnemyMovement>();
        if (enemyMovement != null)
        {
            // Set the printer as the target for the enemy to move towards
            enemyMovement.SetTarget(printerLocation);
        }
    }

    // This method is called by the enemy's movement script when it reaches the printer
    public void OnEnemyReachedPrinter()
    {
        StartCoroutine(DespawnAfterDelay());
    }

    IEnumerator DespawnAfterDelay()
    {
        // Wait for the specified time
        yield return new WaitForSeconds(waitTimeAtPrinter);

        // Instantiate another smoke bomb before despawning
        Instantiate(smokeBombPrefab, spawnedEnemy.transform.position, Quaternion.identity);

        // Destroy the enemy
        Destroy(spawnedEnemy);
    }
}
