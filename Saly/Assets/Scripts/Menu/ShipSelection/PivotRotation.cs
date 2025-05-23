using UnityEngine;

public class PivotRotation : MonoBehaviour
{
    [SerializeField] float rotationSpeed = 0.2f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.RotateAround(new Vector3(960, 540, 0), new Vector3(0, -1, 0), rotationSpeed * Time.deltaTime);
    }
}
