using UnityEngine;

public class AIController : MonoBehaviour
{
    [SerializeField] private string checkpointTag = "Checkpoint"; // Tag for checkpoint objects
    [SerializeField] private float speed = 10f; // Movement speed of the bot
    [SerializeField] private float rotationSpeed = 5f; // Rotation speed of the bot

    private Transform[] checkpoints; // Array to store checkpoint transforms
    private int currentCheckpointIndex = 0;

    private void Start()
    {
        // Find all checkpoint instances in the scene by tag
        GameObject[] checkpointObjects = GameObject.FindGameObjectsWithTag(checkpointTag);
        checkpoints = new Transform[checkpointObjects.Length];

        for (int i = 0; i < checkpointObjects.Length; i++)
        {
            checkpoints[i] = checkpointObjects[i].transform;
        }

        // Sort checkpoints by their name or position if needed
        // Example: Sort by name (assuming names like "Checkpoint1", "Checkpoint2", etc.)
        System.Array.Sort(checkpoints, (a, b) => a.name.CompareTo(b.name));
    }

    private void Update()
    {
        if (checkpoints == null || checkpoints.Length == 0) return; // Ensure checkpoints are assigned

        // Get the current checkpoint
        Transform targetCheckpoint = checkpoints[currentCheckpointIndex];

        // Move towards the checkpoint
        Vector3 direction = (targetCheckpoint.position - transform.position).normalized;
        transform.position += direction * speed * Time.deltaTime;

        // Rotate towards the checkpoint
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        // Check if the bot has reached the checkpoint
        if (Vector3.Distance(transform.position, targetCheckpoint.position) < 1f)
        {
            // Move to the next checkpoint
            currentCheckpointIndex = (currentCheckpointIndex + 1) % checkpoints.Length;
        }
    }
}