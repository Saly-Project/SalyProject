using UnityEngine;

public class ForceField : MonoBehaviour
{
    bool IsActive = false;

    public GameObject Shield;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        IsActive = true; // TO REMOVE
        GameObject.Instantiate(Shield);
    }

    void ToggleShield()
    {
        IsActive = !IsActive;
        Shield.SetActive(IsActive);
    }

    // Update is called once per frame
    void Update()
    {
        Shield.transform.position = this.transform.position;
        if (Input.GetKeyDown(KeyCode.E))
            ToggleShield();
    }
}
