using UnityEngine;

public class SelectAnimation : MonoBehaviour
{
    [SerializeField] private Vector3 finalPosition;
    [SerializeField] private float animationSpeed;
    private Vector3 initialPosition;

    private void Awake()
    {
        initialPosition = transform.position;
    }

    private void Update()
    {
        transform.position = Vector3.Lerp(transform.position, finalPosition, animationSpeed);
    }

    private void OnDisable()
    {
        transform.position = initialPosition;
    }
}
