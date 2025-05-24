using UnityEngine;
using Photon.Pun;
using System;

public class WeaponShooting : MonoBehaviourPun
{
    public Camera cam;
    [SerializeField] private GameObject projectile;
    [SerializeField] private AudioClip projectileFX;
    public Transform LeftFirePoint;
    public Transform RightFirePoint;

    private Vector3 destination;
    private bool leftSide;
    private float timeToFire;
    public float fireRate = 4;

    public int damage;
    public float maxRange = 65f;
    public LayerMask weaponHitLayers;

    void Update()
    {
        if (!photonView.IsMine) return;

        if (!PauseMenu.isPaused && Input.GetButton("Fire") && Time.time >= timeToFire && !GetComponent<ShipController>().isFrozen)
        {
            timeToFire = Time.time + 1 / fireRate;
            AudioSource.PlayClipAtPoint(projectileFX, transform.position, 0.5f);
            Fire();
        }
    }

    void Fire()
    {
        Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        if (!Physics.Raycast(ray, out RaycastHit hit, maxRange, weaponHitLayers))
        {
            destination = ray.GetPoint(1000);
        }
        else
        {
            destination = hit.point;

            // damage relative to distance
            float distance = Vector3.Distance(transform.position, hit.point);
            float normalizedDistance = Mathf.Clamp01((distance - 7.5f) / (maxRange - 7.5f));
            damage = (int)Mathf.Lerp(10, 1, normalizedDistance);

            if (hit.transform.TryGetComponent(out PhotonView targetPV))
            {
                targetPV.RPC("TakeDamageRPC", targetPV.Owner, damage);
            }

        }

        // Spawn the projectile on all clients
        //photonView.RPC("Shoot", RpcTarget.All, LeftFirePoint.position, LeftFirePoint.rotation, RightFirePoint.position, RightFirePoint.rotation, leftSide);
        photonView.RPC("Shoot", RpcTarget.All,
    LeftFirePoint.position, LeftFirePoint.rotation,
    RightFirePoint.position, RightFirePoint.rotation,
    leftSide, destination); leftSide = !leftSide;
    }


    [PunRPC]
    void Shoot(Vector3 leftPos, Quaternion leftRot, Vector3 rightPos, Quaternion rightRot, bool useLeft, Vector3 destination)
    {
        Vector3 spawnPos = useLeft ? leftPos : rightPos;
        Quaternion spawnRot = useLeft ? leftRot : rightRot;

        GameObject projectileInstance = Instantiate(projectile, spawnPos, spawnRot);
        Rigidbody rb = projectileInstance.GetComponent<Rigidbody>();
        rb.linearVelocity = (destination - spawnPos).normalized * 300f;
    }
}
