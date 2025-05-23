using Unity.Netcode;
using UnityEngine;

public class PlayerNetwork : NetworkBehaviour
{
    [SerializeField] GameObject playerCamera;  // r�f�rence � la cam�ra enfant du prefab

    public override void OnNetworkSpawn()
    {
        if (IsOwner)
        {
            // Active la cam�ra du joueur local
            playerCamera.SetActive(true);

            // Parent la cam�ra � ce joueur si besoin
            Camera.main.transform.SetParent(transform);
            Camera.main.transform.localPosition = Vector3.zero;
        }
        else
        {
            // D�sactive la cam�ra sur les instances distantes
            playerCamera.SetActive(false);
        }
    }
}
