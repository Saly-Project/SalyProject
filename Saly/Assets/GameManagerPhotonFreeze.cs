using UnityEngine;
using TMPro;
using Photon.Pun;
using Photon.Realtime;

public class GameManagerPhotonFreeze : MonoBehaviourPunCallbacks
{
    [Header("UI")]
    public GameObject lobbyCanvas;
    public TextMeshProUGUI waitingText;
    public TextMeshProUGUI countdownText;

    [Header("Config")]
    public int maxPlayers = 4;
    public float countdownDuration = 3f;

    private float countdown;
    private bool gameStarted = false;

    void Start()
    {
        countdown = countdownDuration;
        FreezeAllPlayers(true);
        ShowWaitingUI(true);
    }

    void Update()
    {
        if (gameStarted) return;

        if (PhotonNetwork.CurrentRoom.PlayerCount < maxPlayers)
        {
            countdown = countdownDuration;
            Debug.Log("Not enough players");
            waitingText.text = "En attente de joueurs...";
            countdownText.text = "";
        }
        else
        {
            countdown -= Time.deltaTime;
            Debug.Log("Starting");
            waitingText.text = "";
            countdownText.text = Mathf.CeilToInt(countdown).ToString();

            if (countdown <= 0f)
            {
                StartGame();
            }
        }
    }

    void StartGame()
    {
        gameStarted = true;
        FreezeAllPlayers(false);
        ShowWaitingUI(false);
    }

    void FreezeAllPlayers(bool state)
    {
        foreach (GameObject go in GameObject.FindGameObjectsWithTag("Player"))
        {
            var controller = go.GetComponent<ShipController>();
            if (controller != null)
            {
                controller.isFrozen = state;
                controller.enabled = !state;
            }
        }
    }

    void ShowWaitingUI(bool state)
    {
        if (lobbyCanvas != null)
            lobbyCanvas.SetActive(state);
    }
}
