using System.Collections.Generic;
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
    private bool isInLobby = false; // ✅ Track connection state
    private Dictionary<string, RoomInfo> cachedRoomList = new Dictionary<string, RoomInfo>();


    void Start()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        createRoomButton.onClick.AddListener(OnCreateRoomClicked);
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("✅ Connected to Master. Joining lobby...");
        PhotonNetwork.JoinLobby(); // ✅ only after this can we create rooms
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("✅ Joined Lobby.");
        isInLobby = true;
    }

    public void OnCreateRoomClicked()
    {
        Debug.Log("🟢 Clicked Create Room");

        // 🔒 Photon-level readiness check
        if (!PhotonNetwork.IsConnectedAndReady)
        {
            Debug.LogWarning("⚠️ Photon is not ready yet. Wait for OnConnectedToMaster.");
            return;
        }

        // 🔒 App-level flag check
        if (!UsernameFlowManager.JoinedLobby)
        {
            Debug.LogWarning("⚠️ You're not in the lobby yet! Wait until OnJoinedLobby is triggered.");
            return;
        }

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

    public override void OnJoinedRoom()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            Debug.Log("✅ Successfully joined room, loading map: " + selectedMap);
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

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        foreach (RoomInfo info in roomList)
        {
            if (info.RemovedFromList || info.PlayerCount >= info.MaxPlayers)
            {
                if (cachedRoomList.ContainsKey(info.Name))
                    cachedRoomList.Remove(info.Name);
            }
            else
            {
                cachedRoomList[info.Name] = info;
            }
        }

        UpdateRoomListUI();
    }

    void UpdateRoomListUI()
    {
        Debug.Log("🔁 Updating Room List... Total: " + cachedRoomList.Count);

        foreach (Transform child in roomListContainer)
        {
            Destroy(child.gameObject);
        }

        foreach (RoomInfo room in cachedRoomList.Values)
        {
            Debug.Log("🧩 Room: " + room.Name + " (" + room.PlayerCount + "/" + room.MaxPlayers + ")");

            GameObject entry = Instantiate(roomListEntryPrefab, roomListContainer);

            // ✅ Use Legacy Text
            entry.transform.Find("RoomName").GetComponent<Text>().text = room.Name;
            entry.transform.Find("RoomSpace").GetComponent<Text>().text = room.PlayerCount + " / " + room.MaxPlayers;

            Button joinButton = entry.transform.Find("JoinButton").GetComponent<Button>();
            string roomName = room.Name;
            joinButton.onClick.AddListener(() => OnJoinRoom(roomName));
        }
    }

    public void OnJoinRoom(string roomName)
    {
        Debug.Log("🔁 Joining room: " + roomName);
        PhotonNetwork.JoinRoom(roomName);
    }

}
