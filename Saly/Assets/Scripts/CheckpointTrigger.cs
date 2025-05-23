using UnityEngine;

public class CheckpointTrigger : MonoBehaviour
{
    private CheckpointManager manager;
    [SerializeField] private AudioClip CheckpointFX;

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
            AudioSource.PlayClipAtPoint(CheckpointFX, other.transform.position, 0.5f);
            manager.NextCheckpoint();
        }
    }
}
