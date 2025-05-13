using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{

    public GameObject blurPanel;

    private bool isCursorVisible = false;
    public static bool isPaused = false;
    public GameObject PauseMenuUI;
    public GameObject PlayerUI;
    public GameObject SettingsMenuUI;

    public Slider slider;
    public Volume volume;                      // Référence au Volume global
    private DepthOfField depthOfField;  





    void Start(){
        isPaused = false;

        
    }
    

    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)){
            if (isPaused){
                Resume();
            }
            else {
                Pause();
                
            }
        }
    }

    public void Resume()
    {
        PauseMenuUI.SetActive(false);
        PlayerUI.SetActive(true);
        SettingsMenuUI.SetActive(false);
        isPaused = false;
        ToggleCursor();

        //depthOfField.active = false;
        
    }
    

    public void Pause()
    {
        PauseMenuUI.SetActive(true);
        PlayerUI.SetActive(false);

        isPaused = true;
        ToggleCursor();

        //depthOfField.active = true;
        



    }

    public void back(){
        PauseMenuUI.SetActive(true);
        SettingsMenuUI.SetActive(false);
    }

    public void Settings(){
        PauseMenuUI.SetActive(false);
        SettingsMenuUI.SetActive(true);

    }

    void ToggleCursor()
    {
        isCursorVisible = !isCursorVisible; 

        Cursor.visible = isCursorVisible; 
        Cursor.lockState = isCursorVisible ? CursorLockMode.None : CursorLockMode.Locked;

        Debug.Log("Cursor state: " + (isCursorVisible ? "Visible" : "Hidden"));
    }

    void OnApplicationFocus(bool hasFocus)
    {
        // Empêche d'autres scripts ou événements de masquer le curseur lorsqu'on revient dans l'application
        if (isCursorVisible && hasFocus)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }
}
