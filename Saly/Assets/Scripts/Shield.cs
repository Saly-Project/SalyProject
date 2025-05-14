using UnityEngine;

public class ForceField : MonoBehaviour
{
    public bool Charged;
    public float Duration;
    public int MaxCollisions;
    public GameObject Shield;
    public GameObject UIshield;
    public GameObject RechargeVFX;

    bool IsActive = false;
    float ShieldChrono = 0;
    int Collisions = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        UIshield.SetActive(Charged);
        Shield.SetActive(IsActive);
    }

    void ActivateShield()
    {
        if (Charged) // if the skill can be enabled
        {
            IsActive = true;
            Charged = false;
            ShieldChrono = Duration;
            Collisions = 0;
            Shield.SetActive(true);
            UIshield.SetActive(false);
        }
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
            ActivateShield();
        }

        if (IsActive)
        {
            ShieldChrono -= Time.deltaTime;

            if (ShieldChrono <= 0)
            {
                DesactivateShield();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Shield Recharge"))
        {
            if (!Charged)
            {
                Charged = true;
                Destroy(other.gameObject);
                UIshield.SetActive(true);
                var recharge = Instantiate(RechargeVFX, other.transform.position, Quaternion.identity) as GameObject;
                Destroy(recharge, 2f);
            }
        }
        else
        {
            Collisions++;
            if (Collisions >= MaxCollisions)
            {
                DesactivateShield();
            }
        }
    }
}
