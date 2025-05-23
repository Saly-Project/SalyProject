using System;
using System.Runtime.InteropServices;
using UnityEngine;

public class Shield : MonoBehaviour
{
    public bool Charged;
    public float Duration;
    public GameObject ObjShield;
    public GameObject UIshield;
    public GameObject RechargeVFX;

    bool IsActive = false;
    float ShieldClock = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Instantiate(ObjShield);
        UIshield.SetActive(Charged);
        ObjShield.SetActive(IsActive);
    }

    void ActivateShield()
    {
        IsActive = true;
        Charged = false;
        ShieldClock = Duration;
        ObjShield.SetActive(true);
        UIshield.SetActive(false);
    }

    void DesactivateShield()
    {
        IsActive = false;
        ObjShield.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        ObjShield.transform.position = this.transform.position; // the shield follows the player's position

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (Charged) // if the skill can be enabled
            {
                ActivateShield();
            }
        }

        if (IsActive)
        {
            ShieldClock -= Time.deltaTime;

            if (ShieldClock <= 0)
            {
                DesactivateShield();
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
                Destroy(other);
                UIshield.SetActive(true);
                var recharge = Instantiate(RechargeVFX, other.transform.position, Quaternion.identity) as GameObject;
                Destroy(recharge, 2f);
            }
        }
    }
}
