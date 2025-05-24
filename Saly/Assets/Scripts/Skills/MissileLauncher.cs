using System.Collections;
using System.Runtime.InteropServices;
using Photon.Pun;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class MissileLauncher : Skill
{
    public float searchRadius;

    public Transform firePoint;

    public GameObject missilePrefab;
    public GameObject RechargeVFX;

    private void Start()
    {
        UIskill.SetActive(Charged);
    }

    [PunRPC]
    void LauchMissile()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Player");
        GameObject nearestEnemy = null;
        float shortestDistance = Mathf.Infinity;

        foreach (GameObject enemy in enemies)
        {
            if (enemy != gameObject)
            {
                float dist = Vector3.Distance(transform.position, enemy.transform.position);
                if (dist < shortestDistance)
                {
                    nearestEnemy = enemy;
                    shortestDistance = dist;
                }
            }
        }

        if (nearestEnemy != null && shortestDistance <= searchRadius)
        {
            Charged = false;
            UIskill.SetActive(false);
            GameObject missile = Instantiate(missilePrefab, firePoint.position, firePoint.rotation);
            missile.SendMessage("SetTarget", nearestEnemy);
        }
        else
        {
            // UI : no enemies in range
        }
    }

    void Update()
    {
        // Exécuter uniquement sur le joueur local
        if (!photonView.IsMine) return;

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (Charged) // if the skill can be enabled
            {
                photonView.RPC("LauchMissile", RpcTarget.All);
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
