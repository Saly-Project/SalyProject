using Photon.Pun;
using UnityEngine;

public class PlayerSpawner : MonoBehaviourPunCallbacks
{
    public GameObject playerPrefab;

    void Start()
    {
        if (!PhotonNetwork.IsConnected || !PhotonNetwork.InRoom) return;

        // Get spawn points
        Transform[] spawnPoints = GameObject.Find("SpawnPoint")?.GetComponentsInChildren<Transform>();

        if (spawnPoints == null || spawnPoints.Length <= 1)
        {
            Debug.LogError("No spawn points found! Make sure 'Spawns' has children.");
            return;
        }

        int index = PhotonNetwork.LocalPlayer.ActorNumber % (spawnPoints.Length - 1);
        Transform spawnPoint = spawnPoints[index + 1]; // skip the parent

        PhotonNetwork.Instantiate(playerPrefab.name, spawnPoint.position, spawnPoint.rotation);
    }
}
