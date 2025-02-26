using UnityEngine;
using Unity.Netcode;
using System;

public class WeaponShooting : NetworkBehaviour
{

    public Camera cam;
    [SerializeField] private GameObject projectile;
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
        if (!PauseMenu.isPaused && Input.GetButton("Fire") && Time.time >= timeToFire){
            timeToFire = Time.time + 1/fireRate;
            Fire();
        }
    }

    public void Fire(){
        Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        InstantiateProjectile();


        if (!Physics.Raycast(ray, out RaycastHit hit, maxRange, weaponHitLayers)){
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
            Debug.Log(damage + "damages");
        }

        
        
        
    }


    private void InstantiateProjectile()
    {
        // Shoot Bullet Visuals
        if (leftSide){
            leftSide = false;
            GameObject projectileObj = Instantiate(projectile, LeftFirePoint.position, LeftFirePoint.rotation);
            

        }
            
        else {
            leftSide = true;
            GameObject projectileObj = Instantiate(projectile, RightFirePoint.position, RightFirePoint.rotation);
            
        }
    }

    
}
