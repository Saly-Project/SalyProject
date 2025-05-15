using UnityEngine;

public class CheckpointTrigger : MonoBehaviour
{
    private CheckpointManager manager;

    void Start()
    {
        // Trouve le manager dans le parent
        manager = GetComponentInParent<CheckpointManager>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Checkpoint triggered by Player");
            manager.NextCheckpoint();
        }
    }
}
