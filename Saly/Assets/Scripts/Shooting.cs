using UnityEngine;
using Unity.Netcode;
using System;

public class WeaponShooting : NetworkBehaviour
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
    private Transform _cameraTransform;








    void Update()
    {
        if (!IsOwner) return;

        if (!PauseMenu.isPaused && Input.GetButton("Fire") && Time.time >= timeToFire){
            timeToFire = Time.time + 1/fireRate;
            AudioSource.PlayClipAtPoint(projectileFX, transform.position, 0.5f);
            Fire();
        }
    }



    public void Fire()
    {
        Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        ShootServerRpc();


        if (!Physics.Raycast(ray, out RaycastHit hit, maxRange, weaponHitLayers))
        {
            destination = ray.GetPoint(1000);
            return;
        }

        destination = hit.point;

        // damage relative to distance
        float distance = Vector3.Distance(transform.position, hit.point);
        float normalizedDistance = Mathf.Clamp01((distance - 7.5f) / (maxRange - 7.5f));
        damage = (int)Mathf.Lerp(10, 1, normalizedDistance);


        if (hit.transform.TryGetComponent(out PlayerHealth health))
        {
            health.TakeDamageServerRpc(damage);
            Debug.Log(damage + "damages inflicted");
        }
        

    }



    


    [ServerRpc]
    public void ShootServerRpc()
    {
        
        Vector3 offsetViseur = new Vector3(0.5f, 0.5f, 0); // Déplacer légèrement vers le haut du centre

        // Récupérer la direction à partir de la position du viseur
        Ray ray = cam.ViewportPointToRay(offsetViseur);
        Vector3 direction = ray.direction;


        if (leftSide){
            leftSide = false;
            GameObject projectileInstance = Instantiate(projectile, LeftFirePoint.position, LeftFirePoint.rotation);
            Rigidbody rb = projectileInstance.GetComponent<Rigidbody>();
            rb.linearVelocity = direction * 300; // linearVelocity -> velocity
    
            NetworkObject networkObject = projectileInstance.GetComponent<NetworkObject>();
            networkObject.Spawn();

        }
            
        else {
            leftSide = true;
            GameObject projectileInstance = Instantiate(projectile, RightFirePoint.position, RightFirePoint.rotation);
            Rigidbody rb = projectileInstance.GetComponent<Rigidbody>();
            rb.linearVelocity = direction * 300; // linearVelocity -> velocity
    
            NetworkObject networkObject = projectileInstance.GetComponent<NetworkObject>();
            networkObject.Spawn();
        }
        
        
    }


    
}
