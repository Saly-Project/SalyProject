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

    private bool connectedToLobby = false;

    void Start()
    {
        usernamePanel.SetActive(false);
        lobbyPanel.SetActive(false);
        mainMenuPanel.SetActive(true);
        confirmButton.interactable = false;
        loadingScreen.SetActive(false);

        if (statusText != null)
            statusText.text = "";
    }

    public void OnUsernameChanged()
    {
        confirmButton.interactable = !string.IsNullOrWhiteSpace(usernameInput.text);
    }

    public void OnConfirmUsername()
    {
        string playerName = usernameInput.text.Trim();

        if (string.IsNullOrEmpty(playerName))
        {
            if (statusText != null)
                statusText.text = "Please enter a username.";
            return;
        }

        PhotonNetwork.NickName = playerName;

        // Show loading screen
        usernamePanel.SetActive(false);
        mainMenuPanel.SetActive(false);
        loadingScreen.SetActive(true);

        if (spinnerController != null)
            spinnerController.ShowLoading();

        if (statusText != null)
            statusText.text = "Connecting...";

        if (PhotonNetwork.IsConnectedAndReady)
        {
            if (statusText != null)
                statusText.text = "Already connected. Joining lobby...";
            PhotonNetwork.JoinLobby();
        }
        else
        {
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    public override void OnConnectedToMaster()
    {
        if (statusText != null)
            statusText.text = "Connected to Master. Joining Lobby...";

        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        if (spinnerController != null)
            spinnerController.HideLoading();

        loadingScreen.SetActive(false);
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
}
