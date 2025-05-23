using UnityEngine;

public class AsteroidSpawner : MonoBehaviour
{
    public GameObject checkpointsParent;
    public GameObject[] asteroidPrefabs;

    public float interpolationDistance = 5f;
    public int asteroidsPerPoint = 3;
    public float tunnelRadius = 6f;

    [Tooltip("Rayon d'exclusion autour et devant les checkpoints")]
    public float safeRadius = 10f;


    // particles
    public GameObject particleHintPrefab; // à assigner dans l'inspecteur
    public float particleSpacing = 25f;   // distance entre chaque particule


    void Start()
    {
        if (checkpointsParent == null || asteroidPrefabs == null || asteroidPrefabs.Length == 0)
        {
            Debug.LogError("Checkpoints ou prefabs non assignés !");
            return;
        }

        int checkpointCount = checkpointsParent.transform.childCount;
        if (checkpointCount < 2)
        {
            Debug.LogError("Il faut au moins 2 checkpoints !");
            return;
        }

        for (int i = 0; i < checkpointCount - 1; i++)
        {
            Vector3 startPos = checkpointsParent.transform.GetChild(i).position;
            Vector3 endPos = checkpointsParent.transform.GetChild(i + 1).position;

            float segmentLength = Vector3.Distance(startPos, endPos);
            Vector3 direction = (endPos - startPos).normalized;

            int pointsCount = Mathf.Max(1, Mathf.CeilToInt(segmentLength / interpolationDistance));

            for (int p = 0; p <= pointsCount; p++)
            {
                Vector3 pointOnLine = startPos + direction * (p * interpolationDistance);

                for (int a = 0; a < asteroidsPerPoint; a++)
                {
                    Vector3 spawnPos;
                    float scale = Random.Range(400f, 2200f);
                    int tries = 0;

                    do
                    {
                        Vector3 randomOffset = Random.insideUnitSphere.normalized * Random.Range(tunnelRadius * 0.5f, tunnelRadius);
                        spawnPos = pointOnLine + randomOffset;
                        tries++;
                        if (tries > 10) break;
                    }
                    while (IsTooCloseToAnyCheckpoint(spawnPos, scale) || IsInFrontOfAnyCheckpoint(spawnPos, scale));

                    GameObject prefabToSpawn = asteroidPrefabs[Random.Range(0, asteroidPrefabs.Length)];
                    GameObject asteroid = Instantiate(prefabToSpawn, spawnPos, Random.rotation);
                    asteroid.transform.localScale = Vector3.one * scale;

                    Rigidbody rb = asteroid.GetComponent<Rigidbody>();
                    if (rb != null)
                    {
                        // Masse proportionnelle au volume (≈ scale^3)
                        rb.mass = Mathf.Pow(scale, 1.25f) * 0.01f; // ajuste le facteur si c’est trop lourd/léger
                    }

                    
                }

                // Spawner les particules guides à intervalle régulier
                if (particleHintPrefab != null)
                {
                    int particleCount = Mathf.FloorToInt(segmentLength / particleSpacing);

                    for (int pi = 0; pi <= particleCount; pi++)
                    {
                        float t = (pi * particleSpacing) / segmentLength;
                        Vector3 basePos = Vector3.Lerp(startPos, endPos, t);

                        // Offset aléatoire initial autour du chemin (sphere radius 1.5f max)
                        Vector3 randomOffset = Random.insideUnitSphere * Random.Range(15f, 25f);

                        Vector3 spawnPos = basePos + randomOffset;

                        GameObject particle = Instantiate(particleHintPrefab, spawnPos, Quaternion.identity);

                        float randomScale = Random.Range(0.1f, 1f);
                        particle.transform.localScale = Vector3.one * randomScale;

                     // Si tu veux que la particule bouge un peu en flottant, ajoute un script sur elle qui fera une petite oscillation dans Update()
                    }
                }
                
            }
        }

        Debug.Log("Tunnel d'astéroïdes spawné !");


        

    }

    bool IsTooCloseToAnyCheckpoint(Vector3 position, float asteroidSize)
    {
        float buffer = safeRadius * 1.5f + asteroidSize;
        int checkpointCount = checkpointsParent.transform.childCount;

        for (int i = 0; i < checkpointCount; i++)
        {
            Vector3 checkpointPos = checkpointsParent.transform.GetChild(i).position;
            if (Vector3.Distance(position, checkpointPos) < buffer)
                return true;
        }

        return false;
    }

    bool IsInFrontOfAnyCheckpoint(Vector3 position, float asteroidSize)
    {
        float buffer = safeRadius * 1.2f + asteroidSize;
        int count = checkpointsParent.transform.childCount;

        for (int i = 0; i < count - 1; i++)
        {
            Transform checkpoint = checkpointsParent.transform.GetChild(i);
            Transform next = checkpointsParent.transform.GetChild(i + 1);

            Vector3 forward = (next.position - checkpoint.position).normalized;
            Vector3 toAsteroid = position - checkpoint.position;

            float distance = toAsteroid.magnitude;
            float dot = Vector3.Dot(forward, toAsteroid.normalized);

            if (dot > 0.7f && distance < buffer)
                return true;
        }

        return false;
    }
}
