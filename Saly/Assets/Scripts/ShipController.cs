using System.Collections;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Input = UnityEngine.Input;
using TMPro;
using System;
using UnityEngine.UI;
using Unity.Netcode;
using UnityEngine.SceneManagement;

public class ShipController : NetworkBehaviour
{

    // sound FX 
    [SerializeField] public float FXVolume;
    [SerializeField] private AudioClip boostFX;
    
    

    // smoother camera control
    public float pitchLimit = 90f;  
    private float pitch = 0f; 
    private float yaw = 0f;
    public float smoothing = 0.1f;
    private float smoothX = 0f;
    private float smoothY = 0f;

    // Stamina
    public Image StaminaBar;
    public float Stamina, MaxStamina;
    public float StaminaDecrement = 2f;
    public float ChargeRate;
    private Coroutine rechargeStamina;

    // GameObjects
    public Camera camera;
    public GameObject BoostRecharge;
    public GameObject RechargeVFX;
    public GameObject BoostWave;
    [SerializeField] private GameObject ShipBody;
    [SerializeField] private GameObject LeftTurn;
    [SerializeField] private GameObject RightTurn;
    [SerializeField] private GameObject UpTurn;
    [SerializeField] private GameObject DownTurn;
    [SerializeField] private GameObject Rest;

    // Movements
    public float forwardSpeed = 50f, strafeSpeed = 11f;
    public float activeForwardSpeed, activeStrafeSpeed;
    private float forwardAcceleration = 1.2f, strafeAcceleration = 2f;

    
    [SerializeField] public Slider sensitivitySlider;
    [SerializeField] public float sensitivity = 2;

    [SerializeField] TextMeshProUGUI Hud;
    [SerializeField] TextMeshProUGUI sensitivityValue;

    public float roll;
    public float MouseX;
    public float MouseY;

    public float MoveHorizontal;
    public float MoveVertical;

    [SerializeField] private int PlayerSelfLayer = 6;
    Rigidbody rb;


    public CheckpointManager checkpointManager;
    public CheckpointIndicator indicator;


   


    public override void OnNetworkSpawn()
    {

        base.OnNetworkSpawn();

        if (base.IsOwner){

            gameObject.layer = PlayerSelfLayer;
            foreach (Transform child in transform)
            {
                child.gameObject.layer = PlayerSelfLayer;
            }
        }
        
        else if (!base.IsOwner)
        {
            enabled = false;
            return;
        }
        
                        
       
        
    }

    private void Awake(){
        rb = GetComponent<Rigidbody>();
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

    }


    private void HandleInputs(){
        
        roll = Input.GetAxis("Roll");

        MouseX = Input.GetAxisRaw("Mouse X");
        MouseY = Input.GetAxisRaw("Mouse Y");

        MoveHorizontal = Input.GetAxisRaw("Horizontal");
        MoveVertical = Input.GetAxisRaw("Vertical");
        
        
    }



    public void BackToMainMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    
    void MouseSteer()
    {

        float deltaX = MouseX * sensitivity;
        float deltaY = MouseY * sensitivity;

        smoothX = Mathf.Lerp(smoothX, deltaX, smoothing); 

        smoothY = Mathf.Lerp(smoothY, deltaY, smoothing);

        yaw += smoothX;
        pitch -= smoothY;

        

        pitch = Mathf.Clamp(pitch, -pitchLimit, pitchLimit);

        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0f);
        this.transform.localRotation = rotation;


    }




    void Looking()
    {
        /// Looking left/right
        if (roll < 0 || MouseX > 0 && MouseX < 0.5f)
        {
            ShipBody.transform.rotation = Quaternion.Slerp(ShipBody.transform.rotation, LeftTurn.transform.rotation, 0.02f);
        }
        if (roll == 0 || MouseX == 0)
        {
            ShipBody.transform.rotation = Quaternion.Slerp(ShipBody.transform.rotation, transform.rotation, 0.02f);
        }
        if (roll > 0 || MouseX < 0 && MouseX > -0.5f)
        {
            ShipBody.transform.rotation = Quaternion.Slerp(ShipBody.transform.rotation, RightTurn.transform.rotation, 0.02f);
        }

        /// Looking left/right harder

        if (MouseX > 0.5f)
        {
            ShipBody.transform.rotation = Quaternion.Slerp(ShipBody.transform.rotation, LeftTurn.transform.rotation, 0.05f); // tilt model
        }
        if (MouseX > 0.75f)
        {
            rb.AddTorque(transform.forward * -2f * forwardSpeed/10); // tilt camera
        }
        if (MouseX < -0.5f)
        {
            ShipBody.transform.rotation = Quaternion.Slerp(ShipBody.transform.rotation, RightTurn.transform.rotation, 0.05f); // tilt model
        }
        if (MouseX < -0.75f)
        {
            rb.AddTorque(transform.forward * 2f * forwardSpeed/10); // tilt camera
        }



        /// Looking Up/Down

        if (MouseY > 0.2f)
        {
            ShipBody.transform.rotation = Quaternion.Slerp(ShipBody.transform.rotation, UpTurn.transform.rotation, 0.01f);
        }
        if (MouseY > -0.5f && MouseY < 0.2f)
        {
            ShipBody.transform.rotation = Quaternion.Slerp(ShipBody.transform.rotation, transform.rotation, 0.01f);
        }
        if (MouseY < -0.5f)
        {
            ShipBody.transform.rotation = Quaternion.Slerp(ShipBody.transform.rotation, DownTurn.transform.rotation, 0.01f);
        }



        // rotating camera 

        if (Input.GetKey(KeyCode.A))
        {
            rb.AddTorque(transform.forward * 1f * forwardSpeed/10);
        }
        if (Input.GetKey(KeyCode.D))
        {
            rb.AddTorque(transform.forward * -1f * forwardSpeed/10);
        }
    }


    void Move()
    {

        activeStrafeSpeed = Mathf.Lerp(activeStrafeSpeed, MoveHorizontal * strafeSpeed, strafeAcceleration * Time.deltaTime); //strafe move

        // move forward and slow down when turning
        if (activeStrafeSpeed != 0 && MouseX > 0.5f)
            activeForwardSpeed = Mathf.Lerp(activeForwardSpeed, MoveVertical * (forwardSpeed - activeStrafeSpeed+2), forwardAcceleration * Time.deltaTime);

        else if (activeStrafeSpeed != 0 && MouseX < -0.5f)
            activeForwardSpeed = Mathf.Lerp(activeForwardSpeed, MoveVertical * (forwardSpeed + activeStrafeSpeed-2), forwardAcceleration * Time.deltaTime);

        else
            activeForwardSpeed = Mathf.Lerp(activeForwardSpeed, MoveVertical * forwardSpeed, forwardAcceleration * Time.deltaTime);
        

        transform.position += transform.forward * activeForwardSpeed * Time.deltaTime;
        transform.position += transform.right * activeStrafeSpeed * Time.deltaTime;
    }

    


    
    private float rotationLerpSpeed = 0f;
    private void FixedUpdate(){

        
        if (!PauseMenu.isPaused){
            MouseSteer();
            HandleInputs();
            Move();
            Looking();
            Cursor.lockState = CursorLockMode.Locked;
            rb.constraints = RigidbodyConstraints.None;
        }

        else 
        {
            rb.constraints = RigidbodyConstraints.FreezeRotation;
            
            rotationLerpSpeed = Mathf.Min(rotationLerpSpeed + Time.deltaTime * 0.1f, 0.1f); // Gradually increase speed of rotation to rest rotation.
            ShipBody.transform.rotation = Quaternion.Slerp(ShipBody.transform.rotation, Rest.transform.rotation, rotationLerpSpeed); //rotation to rest rotation.

            if (activeForwardSpeed < 0f) activeForwardSpeed = 0;
            if (activeForwardSpeed > 0f) activeForwardSpeed -= 0.15f;

            if (activeStrafeSpeed < 0f) activeStrafeSpeed = 0;
            if (activeStrafeSpeed > 0f)activeStrafeSpeed -= 0.15f;

            transform.position += transform.forward * Mathf.Lerp(activeForwardSpeed, forwardSpeed, activeForwardSpeed * Time.deltaTime) * Time.deltaTime;
            transform.position += transform.right * Mathf.Lerp(activeStrafeSpeed, strafeSpeed, activeStrafeSpeed * Time.deltaTime) * Time.deltaTime;
        }

        



        UpdateHud();
        

        
            
        


        



        // Boost

        forwardSpeed = 50;

        if (Stamina > 0){

            if (Input.GetButton("Aim") || Input.GetKey(KeyCode.LeftShift) && Input.GetButton("Aim")){  //slow down when aiming or (aiming and boosting)
                forwardSpeed -= 10;
            }

            if (Input.GetKey(KeyCode.LeftShift)){
                
                if (Stamina > 15) forwardSpeed += 20;
            }

            if (Input.GetButton("Boost"))
            {
                Stamina -= StaminaDecrement * Time.deltaTime;
                if (Stamina < 0) Stamina = 0;
                StaminaBar.fillAmount = Stamina / MaxStamina;

                if (rechargeStamina != null) StopCoroutine(rechargeStamina);
                rechargeStamina = StartCoroutine(RechargeStamina());
                
                
            }


        }   


    }


   


    
    private void UpdateHud(){
        
        //speed display
        if (Math.Abs(activeStrafeSpeed) > Math.Abs(activeForwardSpeed))
            Hud.text = (Math.Round( Math.Abs(activeStrafeSpeed*10) / 5.0) * 5).ToString("F0") + " km.h";
        else
            Hud.text = (Math.Round( Math.Abs(activeForwardSpeed*10) / 5.0) * 5).ToString("F0") + " km.h";
        
        
    }


    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Boost Ring")) //going through boost ring
        {
            if (activeForwardSpeed > 0)
            {


                var impact = Instantiate(BoostWave, other.transform.position, Quaternion.identity) as GameObject;
                AudioSource.PlayClipAtPoint(boostFX, transform.position, 1f);
                rb.AddForce(transform.forward * 5000f, ForceMode.Impulse);
                activeForwardSpeed += 10f;
                Destroy(impact, 2f);

                
            }
        }

        if (other.CompareTag("Boost Recharge")) //taking boosts on the map
        {
            Stamina += 25f;
            if (Stamina > MaxStamina) Stamina = MaxStamina;
            StaminaBar.fillAmount = Stamina / MaxStamina;
            Destroy(other.gameObject);
            var recharge = Instantiate(RechargeVFX, other.transform.position, Quaternion.identity) as GameObject;
            Destroy(recharge, 2f);


        }

        

        
    }

    private IEnumerator RechargeStamina(){
        yield return new WaitForSeconds(1f);

        while (Stamina < MaxStamina) 
        {
            Stamina += ChargeRate / 50f;
            if (Stamina > MaxStamina) Stamina = MaxStamina;
            StaminaBar.fillAmount = Stamina / MaxStamina;
            yield return new WaitForSeconds(.05f);
        
        }

        
    }

    public void ChangingSettings(){
        sensitivity = sensitivitySlider.value*1.5f;
        sensitivityValue.text = Math.Round(sensitivitySlider.value, 1).ToString();
        if (Math.Round(sensitivitySlider.value, 1).ToString().Length == 1) sensitivityValue.text += ",0";
        
    }

    


    

    
        
    
}
