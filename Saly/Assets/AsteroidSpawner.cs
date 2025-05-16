using UnityEngine;
using UnityEngine.Splines;

public class AsteroidSpawner : MonoBehaviour
{
    public SplineContainer spline;
    public GameObject asteroidPrefab;
    public int count = 50;
    public float radius = 10f;

    void Start()
    {
        for (int i = 0; i < count; i++)
        {
            float t = (float)i / count;

            SplineUtility.Evaluate(spline.Spline, t, out var pos, out var tangent, out var up);

            Vector3 forward = ((Vector3)tangent).normalized;

            Vector3 globalUp = Vector3.up;
            if (Mathf.Abs(Vector3.Dot(forward, globalUp)) > 0.99f)
                globalUp = Vector3.forward;

            Vector3 right = Vector3.Cross(globalUp, forward).normalized;
            Vector3 orthoUp = Vector3.Cross(forward, right).normalized;

            float angle = ((float)i / count) * Mathf.PI * 2f;

            float dist = radius * (0.7f + 0.3f * Mathf.Sin(i * 3.14f));

            Vector3 localOffset = right * Mathf.Cos(angle) + orthoUp * Mathf.Sin(angle);

            Vector3 finalPos = (Vector3)pos + localOffset * dist;

            Quaternion rot = Random.rotation;

            GameObject asteroid = Instantiate(asteroidPrefab, finalPos, rot, transform);

            float randomScale = Random.Range(0.5f, 1.5f);
            asteroid.transform.localScale = Vector3.one * randomScale;
        }
    }
}
