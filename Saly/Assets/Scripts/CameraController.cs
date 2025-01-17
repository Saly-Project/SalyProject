using UnityEngine;
using UnityEngine.Animations;
using Unity.Netcode;

public class CameraFollow : NetworkBehaviour
{


    public ShipController Player;
    [SerializeField] Transform[] povs;
    [SerializeField] float speed = 10;

    private int index = 0;
    private Vector3 target;
    public Camera cam;
    public float boostFov; 
    public float aimFov; 
    public float normalFov;
    public float speedFov; 
    

    public override void OnNetworkSpawn()
    {
        if (!IsOwner){
            gameObject.SetActive(false);
        }
    }


    void Start(){
        Cursor.lockState = CursorLockMode.Locked;
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

            target = povs[index].position;


            if (!PauseMenu.isPaused && Input.GetButton("Aim") && Time.time >= 1 || Player.Stamina > 0 && Input.GetKey(KeyCode.LeftShift) && Input.GetButton("Aim")){  //zoom when aiming
                cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, aimFov, speedFov*Time.deltaTime);
            }

        
        
    

       

    }

    private void FixedUpdate()
    {
        transform.position = Vector3.MoveTowards(transform.position, target, Time.deltaTime * speed);
        transform.forward = povs[index].forward;
    }
}
