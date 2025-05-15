using UnityEngine;
using Unity.Netcode;

public class AIController : NetworkBehaviour
{
    public Transform[] waypoints; // Waypoints for the track
    public float baseSpeed = 10f; // Base speed
    public float turnSpeed = 5f; // Turning speed
    public float aggressiveness = 1f; // Aggressiveness level (1 = normal, >1 = more aggressive)
    public float reactionTime = 0.5f; // Delay in reacting to changes (lower = faster reaction)
    public float speedVariation = 2f; // Random speed variation

    private int currentWaypointIndex = 0;
    private float currentSpeed;
    private float reactionTimer;

    void Start()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.useGravity = false; // Disable gravity
        }
    }

    public void InitializeAI()
    {
        // Assign waypoints dynamically if needed
        currentSpeed = baseSpeed + Random.Range(-speedVariation, speedVariation);
    }

    private void Update()
    {
        reactionTimer += Time.deltaTime;

        // React to changes only after the reaction time has passed
        if (reactionTimer >= reactionTime)
        {
            MoveTowardsWaypoint();
            reactionTimer = 0f;
        }
    }

    private void MoveTowardsWaypoint()
    {
        if (waypoints.Length == 0) return;

        Transform targetWaypoint = waypoints[currentWaypointIndex];
        Vector3 direction = (targetWaypoint.position - transform.position).normalized;

        // Adjust speed based on aggressiveness
        float adjustedSpeed = currentSpeed * aggressiveness;

        // Move forward
        Vector3 newPosition = transform.position + direction * adjustedSpeed * Time.deltaTime;

        // Rotate towards the waypoint
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        Quaternion newRotation = Quaternion.Slerp(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);

        // Synchronize position and rotation
        UpdateAIPositionServerRpc(newPosition, newRotation);

        // Check if the AI reached the waypoint
        if (Vector3.Distance(transform.position, targetWaypoint.position) < 1f)
        {
            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;

            // Randomize speed slightly at each waypoint
            currentSpeed = baseSpeed + Random.Range(-speedVariation, speedVariation);
        }
    }

    [ServerRpc]
    private void UpdateAIPositionServerRpc(Vector3 position, Quaternion rotation)
    {
        transform.position = position;
        transform.rotation = rotation;
    }
}