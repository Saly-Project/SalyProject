using UnityEngine;

public class LobbiesList : MonoBehaviour
{
    bool IsActive = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        IsActive = true; // TO REMOVE
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
