using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    private Transform targetLocation;  // The location the enemy will move towards
    public float speed = 5f;  // Movement speed

    void Update()
    {
        if (targetLocation != null)
        {
            MoveTowardsTarget();
        }
    }

    public void SetTarget(Transform target)
    {
        targetLocation = target;
    }

    void MoveTowardsTarget()
    {
        // Calculate the direction to the target
        Vector3 direction = (targetLocation.position - transform.position).normalized;

        // Move the enemy in the direction of the target
        transform.position += direction * speed * Time.deltaTime;

        // Rotate the enemy to face the target direction
        transform.rotation = Quaternion.LookRotation(direction);

        // Check if the enemy has reached the target
        if (Vector3.Distance(transform.position, targetLocation.position) < 0.1f)
        {
            // Notify the spawner that the enemy has reached the target
            MysteriousStrangerSpawner spawner = FindObjectOfType<MysteriousStrangerSpawner>();
            if (spawner != null)
            {
                spawner.OnEnemyReachedPrinter();
            }
        }
    }
}
