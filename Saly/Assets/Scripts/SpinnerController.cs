using UnityEngine;

public class SpinnerController : MonoBehaviour
{
    public float rotationSpeed = 180f; // degrees per second

    private bool isSpinning = false;

    void Update()
    {
        if (isSpinning)
        {
            transform.Rotate(Vector3.forward, -rotationSpeed * Time.deltaTime);
        }
    }

    public void ShowLoading()
    {
        gameObject.SetActive(true);
        isSpinning = true;
    }

    public void HideLoading()
    {
        isSpinning = false;
        gameObject.SetActive(false);
    }
}
