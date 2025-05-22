using UnityEngine;

public class AsteroidWanderSmooth : MonoBehaviour
{
    private Vector3 rotationSpeed;
    public float movementRadius = 3f;
    public float moveDuration = 4f; // Durée entre deux positions
    public float transitionSmoothness = 0.5f; // 0 = très rigide, 1 = très lisse

    private Vector3 initialPosition;
    private Vector3 startPos;
    private Vector3 targetPos;
    private float t;

    void Start()
    {
        initialPosition = transform.position;
        PickNewTarget();
        rotationSpeed = new Vector3(Random.Range(1f, 4f), Random.Range(1f, 8f), Random.Range(0.5f, 2f));
    }

    void Update()
    {
        // Rotation
        transform.Rotate(rotationSpeed * Time.deltaTime);

        // Avancement dans le temps
        t += Time.deltaTime / moveDuration;

        // Interpolation lissée
        float smoothedT = Mathf.SmoothStep(0f, 1f, t);
        transform.position = Vector3.Lerp(startPos, targetPos, smoothedT);

        // Changement de cible
        if (t >= 1f)
        {
            PickNewTarget();
        }
    }

    void PickNewTarget()
    {
        t = 0f;
        startPos = transform.position;
        Vector3 randomOffset = Random.insideUnitSphere * movementRadius;
        targetPos = initialPosition + randomOffset;
    }
}
