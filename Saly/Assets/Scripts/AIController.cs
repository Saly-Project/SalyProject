using UnityEngine;

public class AIController : MonoBehaviour
{
    [SerializeField] private float checkpointReachDistance = 2f;
    private float speed = 20f;
    private float baseSpeed = 20f;
    private float speedVariance = 3f;
    [SerializeField] private float turnSpeed = 2f;

    private Transform[] checkpoints;
    private int currentCheckpointIndex = 0;

    public void SetCheckpoints(Transform[] cps)
    {
        checkpoints = cps;
        currentCheckpointIndex = 0;
    }

    public void SetSpeed(float newSpeed)
    {
        speed = newSpeed;
        baseSpeed = newSpeed; // Store for future randomization
    }

    public void SetSpeedVariance(float variance)
    {
        speedVariance = variance;
    }

    private void Update()
    {
        if (checkpoints == null || checkpoints.Length == 0) return;

        Transform target = checkpoints[currentCheckpointIndex];

        Vector3 direction = (target.position - transform.position).normalized;
        if (direction != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, turnSpeed * Time.deltaTime);
        }

        transform.position += transform.forward * speed * Time.deltaTime;

        if (Vector3.Distance(transform.position, target.position) < checkpointReachDistance)
        {
            currentCheckpointIndex = (currentCheckpointIndex + 1) % checkpoints.Length;
            // Randomize speed each time a checkpoint is reached
            speed = baseSpeed + Random.Range(-speedVariance, speedVariance);
        }
    }
}