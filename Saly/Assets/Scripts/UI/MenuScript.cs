using UnityEngine;
using UnityEngine.UI;
using TMPro; // Add this for TextMeshPro


public class MenuScript : MonoBehaviour

{

    public GameObject MainMenuUI;
    public GameObject LobbyMenuUI;
    public GameObject SpaceshipMenuUI;
    public GameObject SettingsMenuUI;
    public GameObject mainMenu;
    public GameObject usernameMenu;

    [SerializeField] GameObject CreateButton;
    [SerializeField] GameObject RoomCreation;

    [SerializeField] GameObject Hyperlane;
    [SerializeField] GameObject Warpway;
    [SerializeField] GameObject Oblivion;
    [SerializeField] GameObject Random;

    [SerializeField] private TextMeshProUGUI soloModeButtonText; // Assign the button's TextMeshPro component in the Inspector

    // Start Main Menu automaticaly
    void Start()
    {
        MainMenuUI.SetActive(true);
    }


    // Main menu buttons
    public void Play()
    {
        MainMenuUI.SetActive(false);
        LobbyMenuUI.SetActive(true);
    }

    public void Spaceship()
    {
        MainMenuUI.SetActive(false);
        SpaceshipMenuUI.SetActive(true);
    }

    public void Settings()
    {
        MainMenuUI.SetActive(false);
        SettingsMenuUI.SetActive(true);
    }


    // Back to main button (does work for all menu UI) 
    public void BackToMain()
    {
        PlayerPrefs.Save();
        LobbyMenuUI.SetActive(false);
        SpaceshipMenuUI.SetActive(false);
        SettingsMenuUI.SetActive(false);
        MainMenuUI.SetActive(true);
    }


    // Host Buttons
    public void StartHost()
    {
        MainMenuUI.SetActive(false);
        Cursor.visible = false;
    }

    public void CreateRoom()
    {
        CreateButton.SetActive(false);
        RoomCreation.SetActive(true);
    }


    // Map selection
    public void HyperlaneNext()
    {
        Hyperlane.SetActive(false);
        Warpway.SetActive(true);
    }

    public void WarpwayNext()
    {
        Warpway.SetActive(false);
        Oblivion.SetActive(true);
    }

    public void WarpwayPrev()
    {
        Warpway.SetActive(false);
        Hyperlane.SetActive(true);
    }

    public void OblivionNext()
    {
        Oblivion.SetActive(false);
        Random.SetActive(true);
    }

    public void OblivionPrev()
    {
        Oblivion.SetActive(false);
        Warpway.SetActive(true);
    }

    public void RandomPrev()
    {
        Random.SetActive(false);
        Oblivion.SetActive(true);
    }



    //
    void OnApplicationFocus(bool hasFocus)
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

    }

    public static bool isSoloMode = false;

    public void ToggleSoloMode()
    {
        isSoloMode = !isSoloMode; // Toggle the value of isSoloMode
        Debug.Log("Solo Mode: " + isSoloMode);

        // Update the button text
        if (soloModeButtonText != null)
        {
            soloModeButtonText.text = isSoloMode ? "Solo : YES" : "Solo : NO";
        }
    }

    public void GoToUsernameMenu()
    {
        mainMenu.SetActive(false);
        usernameMenu.SetActive(true);
    }

}
