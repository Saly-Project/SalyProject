using Photon.Pun;
using Unity.Netcode;
using UnityEngine;

public class Skill : MonoBehaviourPunCallbacks
{
    public bool Charged;
    public GameObject UIskill;

    [PunRPC]
    void Disable()
    {
        Charged = false;
        UIskill.SetActive(false);
    }
}
