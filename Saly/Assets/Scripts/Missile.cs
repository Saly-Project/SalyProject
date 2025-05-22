using UnityEngine;

public class Missile : MonoBehaviour
{
    public float Duration;
    public float speed;
    public float rotateSpeed;
    public int Damage;

    public GameObject ExplosionVFX;

    private GameObject target;

    void SetTarget(GameObject _target)
    {
        target = _target;
    }

    void Update()
    {
        if (target != null)
        {
            Vector3 direction = (target.transform.position - transform.position).normalized;
            Quaternion rotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * rotateSpeed);
            transform.position += transform.forward * speed * Time.deltaTime;
        }

        Duration -= Time.deltaTime;

        if (Duration < 0)
        {
            DestroyMissile();
        }
    }

    void DestroyMissile()
    {
        Destroy(gameObject);
        var explosion = Instantiate(ExplosionVFX, transform.position, Quaternion.identity) as GameObject;
        Destroy(explosion, 2f);
    }

    void OnCollisionEnter(Collision collision)
    {
        PlayerHealth hp = collision.gameObject.GetComponent<PlayerHealth>();
        if (hp != null)
        {
            hp.TakeDamage(Damage);
        }
        DestroyMissile();
    }
}