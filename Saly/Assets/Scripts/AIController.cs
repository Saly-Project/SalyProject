using UnityEngine;

public class AIController : MonoBehaviour
{
    [SerializeField] private float checkpointReachDistance = 2f;
    [SerializeField] private float speed = 20f;
    [SerializeField] private float turnSpeed = 2f;

    private Transform[] checkpoints;
    private int currentCheckpointIndex = 0;

    public void SetCheckpoints(Transform[] cps)
    {
        checkpoints = cps;
        currentCheckpointIndex = 0;
    }

    private void Update()
    {
        if (checkpoints == null || checkpoints.Length == 0) return;

        Transform target = checkpoints[currentCheckpointIndex];

        // Smoothly rotate towards the checkpoint
        Vector3 direction = (target.position - transform.position).normalized;
        if (direction != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, turnSpeed * Time.deltaTime);
        }

        // Move forward
        transform.position += transform.forward * speed * Time.deltaTime;

        // Check if reached the checkpoint
        if (Vector3.Distance(transform.position, target.position) < checkpointReachDistance)
        {
            currentCheckpointIndex = (currentCheckpointIndex + 1) % checkpoints.Length;
        }
    }
}