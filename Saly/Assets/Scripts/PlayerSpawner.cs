using UnityEngine;
using Photon.Pun;

public class PlayerSpawner : MonoBehaviour
{
    public string playerPrefabName = "PlayerShip"; // The prefab in Resources

    void Start()
    {
        if (PhotonNetwork.IsConnectedAndReady)
        {
            PhotonNetwork.Instantiate(playerPrefabName, Vector3.zero, Quaternion.identity);
        }
    }
}
