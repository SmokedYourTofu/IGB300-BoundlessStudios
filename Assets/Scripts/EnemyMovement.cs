using UnityEngine;
using UnityEngine.AI; // Import NavMeshAgent for pathfinding

public class EnemyMovement : MonoBehaviour
{
    private NavMeshAgent agent;
    private Transform target;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        if (target != null)
        {
            agent.SetDestination(target.position);
        }
    }

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }
}
