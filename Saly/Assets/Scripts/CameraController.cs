using Photon.Pun;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public ShipController Player;
    [SerializeField] Transform[] povs;
    [SerializeField] GameObject pov1;
    [SerializeField] float speed = 10;

    private int index = 0;
    private Vector3 targetPos;
    private Quaternion originalRotation;
    public Camera cam;
    public float boostFov, aimFov, normalFov, speedFov;

    public float minShakeSpeed = 25f;
    public float baseShakeIntensity = 0.0001f;
    public float maxShakeIntensity = 0.009f;
    public float shakeSpeed = 0.25f;

    private float currentShakeIntensity = 0f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        originalRotation = transform.localRotation;

        // Disable if not local player
        if (!Player.photonView.IsMine)
        {
            gameObject.SetActive(false);
        }
    }

    void Update()
    {
        if (!Player.photonView.IsMine) return;

        if (Input.GetKeyDown(KeyCode.Alpha4)) { index = 1; speed = 400; }
        if (Input.GetKeyUp(KeyCode.Alpha4)) { index = 0; speed = 400; }

        float targetFOV = normalFov;

        if (!PauseMenu.isPaused && Player.Stamina > 0 && Input.GetKey(KeyCode.LeftShift) && !Input.GetButton("Aim"))
        {
            targetFOV = boostFov;
        }

        if (!PauseMenu.isPaused && Input.GetButton("Aim"))
        {
            targetFOV = aimFov;
        }

        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, targetFOV, speedFov * Time.deltaTime);

        targetPos = povs[index].position;

        float playerSpeed = Player.activeForwardSpeed;
        float targetShake = (playerSpeed > minShakeSpeed) ?
            Mathf.Lerp(baseShakeIntensity, maxShakeIntensity, Mathf.Pow((playerSpeed - minShakeSpeed) / 50f, 4)) : 0f;

        currentShakeIntensity = Mathf.Lerp(currentShakeIntensity, targetShake, Time.deltaTime * shakeSpeed);

        if (currentShakeIntensity > 0.00001f)
        {
            float shakeX = Random.Range(-currentShakeIntensity, currentShakeIntensity) * 0.05f;
            float shakeY = Random.Range(-currentShakeIntensity, currentShakeIntensity) * 0.05f;
            float shakeZ = Random.Range(-currentShakeIntensity, currentShakeIntensity) * 0.03f;

            transform.localRotation = originalRotation * Quaternion.Euler(shakeX, shakeY, shakeZ);
        }
        else
        {
            transform.localRotation = originalRotation;
        }
    }

    void FixedUpdate()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPos, Time.deltaTime * speed);
        transform.forward = povs[index].forward;
        transform.rotation = pov1.transform.rotation;
    }
}
