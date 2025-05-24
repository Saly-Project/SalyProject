using Photon.Pun;
using UnityEngine;

public class CheckpointTrigger : MonoBehaviourPunCallbacks
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
            if (!photonView.IsMine) return;
            Debug.Log("Checkpoint triggered by Player");
            AudioSource.PlayClipAtPoint(CheckpointFX, other.transform.position, 0.5f);
            manager.NextCheckpoint();
        }
    }
}
