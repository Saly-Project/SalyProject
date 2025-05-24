using Photon.Pun;
using UnityEngine;

public class PhotonPlayerSpawner : MonoBehaviourPunCallbacks
{
    public GameObject playerPrefab;

    void Start()
    {
        if (PhotonNetwork.IsConnected && PhotonNetwork.InRoom)
        {
            Vector3 randomPos = new Vector3(Random.Range(-5, 5), 1, Random.Range(-5, 5));
            PhotonNetwork.Instantiate(playerPrefab.name, randomPos, Quaternion.identity);
        }
    }
}
