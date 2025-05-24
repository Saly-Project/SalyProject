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
            PhotonView playerView = other.GetComponent<PhotonView>();
            if (playerView != null && playerView.IsMine)
            {
                Debug.Log("Checkpoint triggered by local player");
                AudioSource.PlayClipAtPoint(CheckpointFX, other.transform.position, 0.5f);
                manager.NextCheckpoint();
            }
        }
    }
}
