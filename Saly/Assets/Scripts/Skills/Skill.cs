using Unity.Netcode;
using UnityEngine;

public class Skill : NetworkBehaviour
{
    public bool Charged;
    public GameObject UIskill;

    public void Disable()
    {
        Charged = false;
        UIskill.SetActive(false);
    }
}
