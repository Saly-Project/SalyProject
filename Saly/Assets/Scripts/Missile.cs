using UnityEngine;

public class Missile : MonoBehaviour
{
    public float speed = 10f;
    public float rotateSpeed = 5f;
    public float searchRadius = 50f;
    public Transform target;

    void Start()
    {
        // Cherche automatiquement l’ennemi le plus proche
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        float shortestDistance = Mathf.Infinity;
        Transform nearest = null;

        foreach (GameObject enemy in enemies)
        {
            float dist = Vector3.Distance(transform.position, enemy.transform.position);
            if (dist < shortestDistance)
            {
                shortestDistance = dist;
                nearest = enemy.transform;
            }
        }

        if (nearest != null && shortestDistance <= searchRadius)
        {
            target = nearest;
        }
    }

    void Update()
    {
        if (target == null) return;

        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion rotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * rotateSpeed);
        transform.position += transform.forward * speed * Time.deltaTime;
    }

    void OnCollisionEnter(Collision collision)
    {
        Health hp = collision.gameObject.GetComponent<Health>();
        if (hp != null) hp.TakeDamage(50);
        Destroy(gameObject);
    }
}