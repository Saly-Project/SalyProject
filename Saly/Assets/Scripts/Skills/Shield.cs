using System.Runtime.InteropServices;
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

    void ActivateShield()
    {
        IsActive = true;
        Charged = false;
        ShieldClock = Duration;
        Shield.SetActive(true);
        UIskill.SetActive(false);
    }

    void DesactivateShield()
    {
        IsActive = false;
        Shield.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        Shield.transform.position = this.transform.position; // the shield follows the player's position

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
                Destroy(other.gameObject);
                UIskill.SetActive(true);
                var recharge = Instantiate(RechargeVFX, other.transform.position, Quaternion.identity) as GameObject;
                Destroy(recharge, 2f);
            }
        }
    }
}
