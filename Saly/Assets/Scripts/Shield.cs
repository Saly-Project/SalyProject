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

    // Update is called once per frame
    void Update()
    {
        Shield.SetActive(IsActive);
        Shield.transform.position = this.transform.position;
        if (Input.GetKeyDown(KeyCode.E))
            IsActive = !IsActive;
    }
}
