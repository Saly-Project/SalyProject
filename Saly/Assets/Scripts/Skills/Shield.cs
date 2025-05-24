using System.Runtime.InteropServices;
using Photon.Pun;
using UnityEngine;

public class ForceField : Skill
{
    public float Duration;
    public GameObject Shield;
    public GameObject RechargeVFX;

    bool IsActive = false;
    float ShieldClock = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        UIskill.SetActive(Charged);
        Shield.SetActive(IsActive);
    }

    [PunRPC]
    void ActivateShield()
    {
        IsActive = true;
        Charged = false;
        ShieldClock = Duration;
        Shield.SetActive(true);
        UIskill.SetActive(false);
    }

    [PunRPC]
    void DesactivateShield()
    {
        IsActive = false;
        Shield.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        Shield.transform.position = this.transform.position; // the shield follows the player's position

        // Exécuter uniquement sur le joueur local
        if (!photonView.IsMine) return;

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (Charged) // if the skill can be enabled
            {
                photonView.RPC("ActivateShield", RpcTarget.All);
            }
        }

        if (IsActive)
        {
            ShieldClock -= Time.deltaTime;

            if (ShieldClock <= 0)
            {
                photonView.RPC("DesactivateShield", RpcTarget.All);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Skill Recharge"))
        {
            if (!Charged)
            {
                Charged = true;
                Destroy(other.gameObject);
                UIskill.SetActive(true);
                var recharge = Instantiate(RechargeVFX, other.transform.position, Quaternion.identity) as GameObject;
                Destroy(recharge, 2f);
            }
        }
    }
}
