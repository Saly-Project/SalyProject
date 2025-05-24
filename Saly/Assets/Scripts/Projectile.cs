using UnityEngine;
using Photon.Pun;

public class Projectile : MonoBehaviourPunCallbacks
{
    public GameObject impactVFX;
    private bool collided;
    [SerializeField] private AudioClip impactFX;

    void Start()
    {
        // OPTIONAL: destroy after time if not hit
        Destroy(gameObject, 3f);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collided) return;

        if (collision.gameObject.CompareTag("Player") || 
            collision.gameObject.CompareTag("Bullet") || 
            collision.gameObject.CompareTag("Boost Ring"))
        {
            return; // ignore
        }

        if (impactVFX != null)
        {
            GameObject impact = Instantiate(impactVFX, collision.contacts[0].point, Quaternion.identity);
            Destroy(impact, 0.5f);
        }

        if (impactFX != null)
        {
            AudioSource.PlayClipAtPoint(impactFX, collision.transform.position, 5f);
        }

        collided = true;
        Destroy(gameObject);
    }
}
