using UnityEngine;
using UnityEngine.Animations;
using Unity.Netcode;

public class CameraFollow : NetworkBehaviour
{


    public ShipController Player;
    [SerializeField] Transform[] povs;
    [SerializeField] GameObject pov1;
    [SerializeField] float speed = 10;

    private int index = 0;
    private Vector3 targetPos;
    private Vector3 targetRot;
    public Camera cam;
    public float boostFov; 
    public float aimFov; 
    public float normalFov;
    public float speedFov; 


    // cam shake 
    public float minShakeSpeed = 25f; // Minimum speed required to start shaking
    public float baseShakeIntensity = 0.0001f; // Extremely subtle shake at low speed
    public float maxShakeIntensity = 0.009f; // Tiny shake at high speed
    public float shakeSpeed = 0.25f; // Very slow, smooth transition
    private Quaternion originalRotation;
    private float currentShakeIntensity = 0f;
    

    public override void OnNetworkSpawn()
    {
        if (!IsOwner){
            gameObject.SetActive(false);
        }
    }


    void Start(){
        Cursor.lockState = CursorLockMode.Locked;
        originalRotation = transform.localRotation;
    }
   

    // Update is called once per frame
    private void Update()
    {
        
            if (Input.GetKeyDown(KeyCode.Alpha4)){
            speed = 400;
            index = 1;
            } 
            if (Input.GetKeyUp(KeyCode.Alpha4)){
            
                index = 0;
                speed = 400;
            }


            if (!PauseMenu.isPaused && Player.Stamina > 0 && Input.GetKey(KeyCode.LeftShift) && !Input.GetButton("Aim")){
            
                speed = 150;
                cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, boostFov, speedFov*Time.deltaTime);
            } 
            else 
            {
            
                speed = 200;
                cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, normalFov, speedFov*Time.deltaTime);
            } 

            targetPos = povs[index].position;


            if (!PauseMenu.isPaused && Input.GetButton("Aim") && Time.time >= 1 || Player.Stamina > 0 && Input.GetKey(KeyCode.LeftShift) && Input.GetButton("Aim")){  //zoom when aiming
                cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, aimFov, speedFov*Time.deltaTime);
            }



            float playerSpeed = Player.activeForwardSpeed;

            float targetShake = (playerSpeed > minShakeSpeed)
            ? Mathf.Lerp(baseShakeIntensity, maxShakeIntensity, Mathf.Pow((playerSpeed - minShakeSpeed) / 50f, 4)) // Smooth, exponential increase
            : 0f; // No shake if below threshold

            // Smoothly transition the shake intensity
            currentShakeIntensity = Mathf.Lerp(currentShakeIntensity, targetShake, Time.deltaTime * shakeSpeed);

            // Apply subtle rotation shake if intensity is perceptible
            if (currentShakeIntensity > 0.00001f) // Only apply shake if it's noticeable
            {
                // Extremely small random shake on each axis (X, Y, Z) for subtle rotation
                float shakeX = Random.Range(-currentShakeIntensity, currentShakeIntensity) * 0.05f;
                float shakeY = Random.Range(-currentShakeIntensity, currentShakeIntensity) * 0.05f;
                float shakeZ = Random.Range(-currentShakeIntensity, currentShakeIntensity) * 0.03f;

                // Apply rotation shake
                transform.localRotation = originalRotation * Quaternion.Euler(shakeX, shakeY, shakeZ);
            }
            else
            {
                transform.localRotation = originalRotation; // Reset if shake is too small
            }

        
        
    

       

    }

    private void FixedUpdate()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPos, Time.deltaTime * speed);
        transform.forward = povs[index].forward;

        transform.rotation = pov1.transform.rotation;
    }
}
