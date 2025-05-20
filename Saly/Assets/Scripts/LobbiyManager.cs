using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

public class LobbiesList : MonoBehaviourPunCallbacks
{
    public InputField createRoomInput;
    public Button createRoomButton;

    public Transform roomListContainer;
    public GameObject roomListEntryPrefab;

    public GameObject hyperlaneUI;
    public GameObject warpwayUI;
    public GameObject oblivionUI;
    public GameObject randomUI;

    private string selectedMap = "Hyperlane";

    void Start()
    {
        createRoomButton.onClick.AddListener(OnCreateRoomClicked);
    }

    public void OnCreateRoomClicked()
    {
        Debug.Log("🟢 Clicked Create Room");

        if (createRoomInput == null)
        {
            Debug.LogError("❌ createRoomInput is NULL! Did you assign it in the Inspector?");
            return;
        }

        Debug.Log("InputField reference is OK");
        Debug.Log("Text inside: '" + createRoomInput.text + "'");

        string roomName = createRoomInput.text.Trim();

        if (string.IsNullOrEmpty(roomName))
        {
            Debug.LogWarning("⚠️ Room name is empty!");
            return;
        }

        Debug.Log("✅ Creating room with name: " + roomName);

        RoomOptions options = new RoomOptions { MaxPlayers = 4 };
        PhotonNetwork.CreateRoom(roomName, options);
    }

    public void OnJoinRoom(string roomName)
    {
        PhotonNetwork.JoinRoom(roomName);
    }

    public override void OnJoinedRoom()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.LoadLevel(selectedMap);
        }
    }

    // MAP SWITCHING METHODS

    public void HyperlaneNext()
    {
        hyperlaneUI.SetActive(false);
        warpwayUI.SetActive(true);
        selectedMap = "Warpway";
    }

    public void WarpwayNext()
    {
        warpwayUI.SetActive(false);
        oblivionUI.SetActive(true);
        selectedMap = "Oblivion";
    }

    public void WarpwayPrev()
    {
        warpwayUI.SetActive(false);
        hyperlaneUI.SetActive(true);
        selectedMap = "Hyperlane";
    }

    public void OblivionNext()
    {
        oblivionUI.SetActive(false);
        randomUI.SetActive(true);
        selectedMap = "Random";
    }

    public void OblivionPrev()
    {
        oblivionUI.SetActive(false);
        warpwayUI.SetActive(true);
        selectedMap = "Warpway";
    }

    public void RandomPrev()
    {
        randomUI.SetActive(false);
        oblivionUI.SetActive(true);
        selectedMap = "Oblivion";
    }
}
