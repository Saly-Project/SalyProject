using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class Crosshair : MonoBehaviour
{
    
    public ShipController Player;
    private RectTransform crosshair;  

    public float restingSize;
    public float maxSize; 
    public float speed;

    private float currentSize;



    private void Start()
    {
        crosshair = GetComponent<RectTransform>();
    }

    private void Update()
    {
        if (!PauseMenu.isPaused){
            if (isBoosting){
                currentSize = Mathf.Lerp(currentSize, maxSize, Time.deltaTime * speed);
            }
            else {
                currentSize = Mathf.Lerp(currentSize, restingSize, Time.deltaTime * speed);
            }

            if (isShooting){
                currentSize = Mathf.Lerp(currentSize, maxSize, Time.deltaTime * speed);
            }
            else {
                currentSize = Mathf.Lerp(currentSize, restingSize, Time.deltaTime * speed);
            }
        }
        

        crosshair.sizeDelta = new Vector2(currentSize, currentSize);

    }

    bool isBoosting  {

        get {
            if (Player.Stamina > 0 && Input.GetKey(KeyCode.LeftShift) && !Input.GetButton("Aim")) return true;
            else return false;
        }
    }

    bool isShooting {
        get {
            if (Input.GetAxis("Fire") != 0) return true;
            else return false;
        }
    }
}
