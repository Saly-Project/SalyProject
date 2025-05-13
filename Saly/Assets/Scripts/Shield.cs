using UnityEngine;

public class ForceField : MonoBehaviour
{
    public bool Charged;
    public float Duration;
    public GameObject Shield;
    public GameObject UIshield;

    bool IsActive = false;
    float ShieldChrono = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        UIshield.SetActive(Charged);
        Shield.SetActive(IsActive);
    }

    void ActivateShield()
    {
        if (Charged)
        {
            Shield.SetActive(true);
            IsActive = true;
            ShieldChrono = 0;
            Charged = false;
            UIshield.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        Shield.transform.position = this.transform.position;

        if (Input.GetKeyDown(KeyCode.E))
            ActivateShield();

        if (IsActive)
        {
            ShieldChrono += Time.deltaTime;
            if (ShieldChrono >= Duration)
            {
                IsActive = false;
                Shield.SetActive(false);
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
                other.gameObject.SetActive(false);
                Destroy(other.gameObject);
                UIshield.SetActive(true);
            }
        }
    }
}
