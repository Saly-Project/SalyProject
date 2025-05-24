using System.Collections;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using TMPro;
using UnityEngine.UI;
using Photon.Pun;
using System;

public class ShipController : MonoBehaviourPunCallbacks
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

    private bool IsSlowed = false;
    private float SlowTimer = 0;
    private float maxSpeed = float.MaxValue;

    public bool isFrozen = true;

    public GameObject WinUI;
    public GameObject ScreenUI;

    public void SetFrozen(bool freeze)
    {
        isFrozen = freeze;
    }


    private void Start()
    {
        rb = GetComponent<Rigidbody>();

        if (!photonView.IsMine)
        {
            enabled = false;
            return;
        }

        // Setup local player stuff
        gameObject.layer = PlayerSelfLayer;
        foreach (Transform child in transform)
        {
            child.gameObject.layer = PlayerSelfLayer;
        }

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        WinUI.SetActive(false);
    }

    private void HandleInputs()
    {
        roll = Input.GetAxis("Roll");
        MouseX = Input.GetAxisRaw("Mouse X");
        MouseY = Input.GetAxisRaw("Mouse Y");
        MoveHorizontal = Input.GetAxisRaw("Horizontal");
        MoveVertical = Input.GetAxisRaw("Vertical");
    }

    public void BackToMainMenu()
    {
        PhotonNetwork.LeaveRoom();
        UnityEngine.SceneManagement.SceneManager.LoadScene("Menu");
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
        transform.localRotation = rotation;
    }

    void Looking()
    {
        if (roll < 0 || (MouseX > 0 && MouseX < 0.5f))
            ShipBody.transform.rotation = Quaternion.Slerp(ShipBody.transform.rotation, LeftTurn.transform.rotation, 0.02f);
        if (roll == 0 || MouseX == 0)
            ShipBody.transform.rotation = Quaternion.Slerp(ShipBody.transform.rotation, transform.rotation, 0.02f);
        if (roll > 0 || (MouseX < 0 && MouseX > -0.5f))
            ShipBody.transform.rotation = Quaternion.Slerp(ShipBody.transform.rotation, RightTurn.transform.rotation, 0.02f);

        if (MouseX > 0.5f)
            ShipBody.transform.rotation = Quaternion.Slerp(ShipBody.transform.rotation, LeftTurn.transform.rotation, 0.05f);
        if (MouseX > 0.75f)
            rb.AddTorque(transform.forward * -2f * forwardSpeed / 10);
        if (MouseX < -0.5f)
            ShipBody.transform.rotation = Quaternion.Slerp(ShipBody.transform.rotation, RightTurn.transform.rotation, 0.05f);
        if (MouseX < -0.75f)
            rb.AddTorque(transform.forward * 2f * forwardSpeed / 10);

        if (MouseY > 0.2f)
            ShipBody.transform.rotation = Quaternion.Slerp(ShipBody.transform.rotation, UpTurn.transform.rotation, 0.01f);
        if (MouseY > -0.5f && MouseY < 0.2f)
            ShipBody.transform.rotation = Quaternion.Slerp(ShipBody.transform.rotation, transform.rotation, 0.01f);
        if (MouseY < -0.5f)
            ShipBody.transform.rotation = Quaternion.Slerp(ShipBody.transform.rotation, DownTurn.transform.rotation, 0.01f);

        if (Input.GetKey(KeyCode.A))
            rb.AddTorque(transform.forward * 1f * forwardSpeed / 10);
        if (Input.GetKey(KeyCode.D))
            rb.AddTorque(transform.forward * -1f * forwardSpeed / 10);
    }

    void Move()
    {
        activeStrafeSpeed = Mathf.Lerp(activeStrafeSpeed, MoveHorizontal * strafeSpeed, strafeAcceleration * Time.deltaTime);

        if (activeStrafeSpeed != 0 && MouseX > 0.5f)
            activeForwardSpeed = Mathf.Lerp(activeForwardSpeed, MoveVertical * (forwardSpeed - activeStrafeSpeed + 2), forwardAcceleration * Time.deltaTime);
        else if (activeStrafeSpeed != 0 && MouseX < -0.5f)
            activeForwardSpeed = Mathf.Lerp(activeForwardSpeed, MoveVertical * (forwardSpeed + activeStrafeSpeed - 2), forwardAcceleration * Time.deltaTime);
        else
            activeForwardSpeed = Mathf.Lerp(activeForwardSpeed, MoveVertical * forwardSpeed, forwardAcceleration * Time.deltaTime);

        if (activeForwardSpeed > maxSpeed)
        {
            activeForwardSpeed = maxSpeed;
        }
        else if (activeForwardSpeed < -maxSpeed)
        {
            activeForwardSpeed = -maxSpeed;
        }

        transform.position += transform.forward * activeForwardSpeed * Time.deltaTime;
        transform.position += transform.right * activeStrafeSpeed * Time.deltaTime;
    }

    [PunRPC]
    public void Slow(float[] data)
    {
        float value = data[0];
        float duration = data[1];

        IsSlowed = true;
        SlowTimer = duration;
        maxSpeed = value;
    }

    private void Update()
    {
        if (IsSlowed)
        {
            SlowTimer -= Time.deltaTime;

            if (SlowTimer < 0)
            {
                IsSlowed = false;
                maxSpeed = float.MaxValue;
            }
        }
    }


    private float rotationLerpSpeed = 0f;
    private void FixedUpdate()
    {
        if (!photonView.IsMine) return;

        if (!PauseMenu.isPaused && !isFrozen)
        {
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
            ShipBody.transform.rotation = Quaternion.Slerp(ShipBody.transform.rotation, Rest.transform.rotation, 0.1f);
        }

        UpdateHud();
        HandleBoosting();
    }

    private void HandleBoosting()
    {
        forwardSpeed = 50;

        if (Stamina > 0)
        {
            if (Input.GetButton("Aim") || Input.GetKey(KeyCode.LeftShift) && Input.GetButton("Aim"))
                forwardSpeed -= 10;

            if (Input.GetKey(KeyCode.LeftShift) && Stamina > 15)
                forwardSpeed += 20;

            if (Input.GetButton("Boost"))
            {
                Stamina -= StaminaDecrement * Time.deltaTime;
                Stamina = Mathf.Max(0, Stamina);
                StaminaBar.fillAmount = Stamina / MaxStamina;

                if (rechargeStamina != null) StopCoroutine(rechargeStamina);
                rechargeStamina = StartCoroutine(RechargeStamina());
            }
        }
    }

    private void UpdateHud()
    {
        float displaySpeed = Mathf.Max(Mathf.Abs(activeStrafeSpeed), Mathf.Abs(activeForwardSpeed)) * 10;
        Hud.text = (Math.Round(displaySpeed / 5.0) * 5).ToString("F0") + " km.h";
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("FinalCheckpoint"))
        {
            WinUI.SetActive(true);
            ScreenUI.SetActive(false);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Boost Ring") && activeForwardSpeed > 0)
        {
            var impact = Instantiate(BoostWave, other.transform.position, Quaternion.identity);
            AudioSource.PlayClipAtPoint(boostFX, transform.position, 1f);
            rb.AddForce(transform.forward * 5000f, ForceMode.Impulse);
            activeForwardSpeed += 10f;
            Destroy(impact, 2f);
        }

        if (other.CompareTag("Boost Recharge"))
        {
            Stamina += 25f;
            Stamina = Mathf.Min(Stamina, MaxStamina);
            StaminaBar.fillAmount = Stamina / MaxStamina;
            Destroy(other.gameObject);
            var recharge = Instantiate(RechargeVFX, other.transform.position, Quaternion.identity);
            Destroy(recharge, 2f);
        }
    }

    private IEnumerator RechargeStamina()
    {
        yield return new WaitForSeconds(1f);
        while (Stamina < MaxStamina)
        {
            Stamina += ChargeRate / 50f;
            Stamina = Mathf.Min(Stamina, MaxStamina);
            StaminaBar.fillAmount = Stamina / MaxStamina;
            yield return new WaitForSeconds(0.05f);
        }
    }

    public void ChangingSettings()
    {
        sensitivity = sensitivitySlider.value * 1.5f;
        sensitivityValue.text = Math.Round(sensitivitySlider.value, 1).ToString();
        if (sensitivityValue.text.Length == 1) sensitivityValue.text += ",0";
    }
}
