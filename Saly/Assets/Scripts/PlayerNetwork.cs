using Unity.Netcode;
using UnityEngine;

public class PlayerNetwork : NetworkBehaviour
{
    [SerializeField] GameObject playerCamera;  // référence à la caméra enfant du prefab

    public override void OnNetworkSpawn()
    {
        if (IsOwner)
        {
            // Active la caméra du joueur local
            playerCamera.SetActive(true);

            // Parent la caméra à ce joueur si besoin
            Camera.main.transform.SetParent(transform);
            Camera.main.transform.localPosition = Vector3.zero;
        }
        else
        {
            // Désactive la caméra sur les instances distantes
            playerCamera.SetActive(false);
        }
    }
}
