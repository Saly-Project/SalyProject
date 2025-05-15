using UnityEngine;

public class MenuCamera : MonoBehaviour
{
    [SerializeField] private Vector3 rotationSpeed = new Vector3(0, 5f, 0); // Adjust speed here (5 degrees per second)

    void Update()
    {
        transform.Rotate(rotationSpeed * Time.deltaTime);
    }
}
