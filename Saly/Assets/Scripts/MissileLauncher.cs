using UnityEngine;

public class MissileLauncher : MonoBehaviour
{
    public GameObject missilePrefab;
    public Transform firePoint;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (missilePrefab != null && firePoint != null)
            {
                Instantiate(missilePrefab, firePoint.position, firePoint.rotation);
            }
        }
    }
}
