using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;

public class PlayerSpawner : MonoBehaviourPunCallbacks
{
    private bool hasSpawned = false;

    void Start()
    {
        // Scene is already loaded, try spawning
        TrySpawn();
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("✅ OnJoinedRoom triggered. Attempting to spawn player.");
        TrySpawn();
    }

    private void TrySpawn()
    {
        if (hasSpawned || !PhotonNetwork.InRoom)
        {
            Debug.Log("⚠ Cannot spawn: Already spawned or not in room.");
            return;
        }

        Transform[] spawnPoints = GameObject.Find("Spawns").GetComponentsInChildren<Transform>();
        if (spawnPoints.Length <= 1)
        {
            Debug.LogError("❌ No spawn points found! Make sure 'Spawns' has children.");
            return;
        }

        int index = PhotonNetwork.LocalPlayer.ActorNumber % (spawnPoints.Length - 1);
        Transform spawnPoint = spawnPoints[index + 1];

        Debug.Log($"🚀 Spawning player at index {index}: ({spawnPoint.position})");
        PhotonNetwork.Instantiate(PlayerPrefs.GetString("spaceship", "Aetherwing"), spawnPoint.position, spawnPoint.rotation);
        hasSpawned = true;
    }
}
