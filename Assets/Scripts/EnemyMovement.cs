using UnityEngine;

public class EnemyMovement : NavigationAgent
{
    private Transform targetLocation;  // The location the enemy will move towards
    public float speed = 5f;  // Movement speed

    //Movement Variables
    private float moveSpeed = 3f;
    public float minDistance = 0.5f;

    //FSM Variables
    public int newState = 0;
    // private int currentState = 0;

    [SerializeField] private SpringJoint joint;

    private Animator animator;

    public AudioClip getSound;
    private SoundManager soundManager;

    private int[,] DFA = {
        { 0, 1, 2 },
        { 0, 1, 2 },
        { 0, 1, 2 }
    };

    void Start()
    {
        //Find waypoint graph
        graphNodes = GameObject.FindGameObjectWithTag("waypoint graph").GetComponent<WaypointGraph>();
        //Initial node index to move to
        currentPath.Add(currentNodeIndex);

        joint = GetComponent<SpringJoint>();

        soundManager = FindObjectOfType<SoundManager>();

        if (this.gameObject.tag == "Untagged")
        {
            animator = GetComponentInChildren<Animator>();
        }
    }

    void Update()
    {
        if (targetLocation != null)
        {
            //MoveTowardsTarget();
            Attack();
            Move();

            if (Vector3.Distance(transform.position, graphNodes.graphNodes[6].transform.position) < 0.1f)
            {
                // Notify the spawner that the enemy has reached the target
                MysteriousStrangerSpawner spawner = FindObjectOfType<MysteriousStrangerSpawner>();
                if (spawner != null)
                {
                    animator.Play("Idle");
                    // Calculate the direction to the target
                    Vector3 direction = (targetLocation.position - transform.position).normalized;

                    // Rotate the enemy to face the target direction
                    transform.rotation = Quaternion.LookRotation(direction);
                    spawner.OnEnemyReachedPrinter();
                }
            }
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
    private void Attack()
    {
        Debug.Log("Attack");

        if (Vector3.Distance(transform.position, graphNodes.graphNodes[6].transform.position) > minDistance && currentPath[currentPath.Count - 1] != 6)
        {
            currentPath = AStarSearch(currentPath[currentPathIndex], 6);
            currentPathIndex = 0;
        }
    }

    private void Move()
    {

        if (currentPath.Count > 0)
        {

            //Move towards next node in path
            transform.position = Vector3.MoveTowards(transform.position, graphNodes.graphNodes[currentPath[currentPathIndex]].transform.position, moveSpeed * Time.deltaTime);

            Vector3 direction = (graphNodes.graphNodes[currentPath[currentPathIndex]].transform.position - transform.position).normalized;

            transform.rotation = Quaternion.LookRotation(direction);

            if (joint != null)
            {
                joint.connectedAnchor = transform.position;
            }

            //Increase path index
            if (Vector3.Distance(transform.position, graphNodes.graphNodes[currentPath[currentPathIndex]].transform.position) <= minDistance)
            {

                if (currentPathIndex < currentPath.Count - 1)
                    currentPathIndex++;
            }

            currentNodeIndex = graphNodes.graphNodes[currentPath[currentPathIndex]].GetComponent<LinkedNodes>().index;   //Store current node index
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        // Check if the player collided with the mysterious stranger
        if (collision.gameObject.CompareTag("Player"))
        {
            // Find the spawner and call the DespawnMysteriousStranger method
            MysteriousStrangerSpawner spawner = FindObjectOfType<MysteriousStrangerSpawner>();
            if (spawner != null)
            {
                soundManager.PlaySoundFXclip(getSound, soundManager.transform, 0.2f);
                spawner.DespawnMysteriousStranger();

                // Optionally, you can add score or handle other player-related interactions here
                ScoreManager.Instance.AddScore(spawner.scoreOnDestroy);
            }
        }
    }
}
