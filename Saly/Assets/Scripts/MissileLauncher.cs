using UnityEngine;

public class MissileLauncher : MonoBehaviour
{
    public GameObject missilePrefab;
    public Transform firePoint;
    public Transform missileTarget;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            GameObject missile = Instantiate(missilePrefab, firePoint.position, firePoint.rotation);
            missile.GetComponent<Missile>().target = missileTarget;
        }
    }
}
