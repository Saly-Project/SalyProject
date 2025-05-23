using Photon.Pun;
using UnityEngine;

public class PlayerSpawner : MonoBehaviourPunCallbacks
{
    public GameObject playerPrefab;

    void Start()
    {
        Transform[] spawnPoints = GameObject.Find("Spawns").GetComponentsInChildren<Transform>();

        int index = PhotonNetwork.LocalPlayer.ActorNumber % (spawnPoints.Length - 1); // avoid using the parent

        Transform spawnPoint = spawnPoints[index + 1]; // +1 to skip the parent

        PhotonNetwork.Instantiate(playerPrefab.name, spawnPoint.position, spawnPoint.rotation);
    }
}
