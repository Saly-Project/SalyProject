using UnityEngine;
using Unity.Netcode;

public class Projectile : NetworkBehaviour
{

    public GameObject impactVFX;
    private bool collided;




    void Start()
    {
        if (!IsOwner) return;
    }


    void OnCollisionEnter(Collision co){
        if (co.gameObject.tag != "Player" && co.gameObject.tag != "Bullet" && !collided && co.gameObject.tag != "Boost Ring")
        {
            var impact = Instantiate ( impactVFX, co.contacts[0].point, Quaternion.identity) as GameObject;
            Destroy(impact, 0.5f);
            collided = true;
            Destroy(gameObject);
        }
    }

    void Update()
    {
        if (!IsOwner) return;
        Destroy(gameObject, 3);
    }
}
