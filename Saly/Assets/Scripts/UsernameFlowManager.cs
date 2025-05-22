using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using UnityEngine.SceneManagement;
using Photon.Realtime;
using System.Collections;

public class UsernameFlowManager : MonoBehaviourPunCallbacks
{
    public GameObject usernamePanel;
    public GameObject lobbyPanel;

    public InputField usernameInput;
    public Button confirmButton;
    public Text statusText;
    public GameObject mainMenuPanel;
    public GameObject loadingScreen;
    public SpinnerController spinnerController;
    public static bool JoinedLobby = false;

    private bool connectedToLobby = false;

    void Start()
    {
        PhotonNetwork.AutomaticallySyncScene = true;         // optional but best practice

        usernamePanel.SetActive(false);
        lobbyPanel.SetActive(false);
        mainMenuPanel.SetActive(true);
        confirmButton.interactable = false;
        loadingScreen.SetActive(false);                       // Show loading UI

        if (statusText != null)
            statusText.text = "🔌 Connecting to Photon...";

        if (PhotonNetwork.IsConnected)
        {
            PhotonNetwork.Disconnect(); // Ensures you reset connection unless triggered manually
        }

    }

    public void OnUsernameChanged()
    {
        confirmButton.interactable = !string.IsNullOrWhiteSpace(usernameInput.text);
    }

    public void OnConfirmUsername()
    {
        Debug.Log("🟡 Button clicked");

        string playerName = usernameInput.text.Trim();

        if (string.IsNullOrEmpty(playerName))
        {
            if (statusText != null)
                statusText.text = "Please enter a username.";
            return;
        }

        PhotonNetwork.NickName = playerName;

        // Show loading screen immediately
        usernamePanel.SetActive(false);
        mainMenuPanel.SetActive(false);
        loadingScreen.SetActive(true);

        if (spinnerController != null)
            spinnerController.ShowLoading();

        if (statusText != null)
            statusText.text = "Connecting...";

        if (!PhotonNetwork.IsConnectedAndReady)
        {
            Debug.LogWarning("❌ Photon is not connected or ready.");
            PhotonNetwork.ConnectUsingSettings(); // ✅ Connect AFTER showing the screen
            return;
        }

        Debug.Log("✅ Photon is ready. Continuing...");
        PhotonNetwork.JoinLobby();
    }


    public override void OnConnectedToMaster()
    {
        if (statusText != null)
            statusText.text = "Connected to Master. Joining Lobby...";

        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        if (!usernamePanel.activeSelf && !loadingScreen.activeSelf)
        {
            Debug.Log("⛔ Prevented auto-joining lobby (not triggered from username flow)");
            return;
        }

        Debug.Log("✅ Joined lobby from username flow");
        JoinedLobby = true;

        // ✅ Proper cleanup
        if (spinnerController != null)
            spinnerController.HideLoading();

        loadingScreen.SetActive(false);
        usernamePanel.SetActive(false); // <--- this line is key
        mainMenuPanel.SetActive(false); // <--- also hide menu just in case
        lobbyPanel.SetActive(true);

        StartCoroutine(ShowLobbyWithDelay());
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        if (spinnerController != null)
            spinnerController.HideLoading();

        if (statusText != null)
            statusText.text = "Connection failed: " + cause.ToString();

        loadingScreen.SetActive(false);
        usernamePanel.SetActive(true);
    }

    public void StartUsernameFlow()
    {
        mainMenuPanel.SetActive(false);
        usernamePanel.SetActive(true);
    }

    public void ReturnToMainMenu()
    {
        PhotonNetwork.LeaveLobby();

        mainMenuPanel.SetActive(true);
        usernamePanel.SetActive(false);
        lobbyPanel.SetActive(false);
        loadingScreen.SetActive(false);

        if (spinnerController != null)
            spinnerController.HideLoading();
    }

    private IEnumerator ShowLobbyWithDelay()
    {
        yield return new WaitForSeconds(1f);

        if (spinnerController != null)
            spinnerController.HideLoading();

        loadingScreen.SetActive(false);
        lobbyPanel.SetActive(true);
    }

    public void TestClick()
{
    Debug.Log("✅ TestClick triggered!");
}

}