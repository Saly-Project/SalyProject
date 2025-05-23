using TMPro;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RoomManager : MonoBehaviour
{
    [SerializeField] Button m_StartHostButton;
    [SerializeField] Button m_StartClientButton;
    public GameObject MenuUI;

    [SerializeField] GameObject Hyperlane;
    [SerializeField] GameObject Warpway;
    [SerializeField] GameObject Oblivion;
    [SerializeField] GameObject Random;

    [SerializeField] TMP_InputField inputField;

    public string MapName = "Hyperlane";


    void Awake()
    {
        // Si aucun transport n’est configuré, on en crée un et on l’assigne
        if (NetworkManager.Singleton.NetworkConfig.NetworkTransport == null)
        {
            var utp = gameObject.AddComponent<UnityTransport>();
            NetworkManager.Singleton.NetworkConfig.NetworkTransport = utp;
        }
    }

    // Start is called before the first frame update
    void Start()
    {

        m_StartHostButton.onClick.AddListener(StartHost);
        m_StartClientButton.onClick.AddListener(StartClient);
    }

    void StartClient()
    {
        NetworkManager.Singleton.StartClient();
        DeactivateButtons();
    }

    public void StartHost()
    {
        MenuUI.SetActive(false);
        NetworkManager.Singleton.StartHost();
        DeactivateButtons();

        if (MapName == "Random")
        {
            System.Random rng = new System.Random();
            int randomValue = rng.Next(1, 4);
            if (randomValue == 1)
                MapName = "Hyperlane";
            else if (randomValue == 2)
                MapName = "Warpway";
            else
                MapName = "Oblivion";
        }

        NetworkManager.Singleton.SceneManager.LoadScene(MapName, LoadSceneMode.Single);
    }

    void DeactivateButtons()
    {
        m_StartHostButton.interactable = false;
        m_StartClientButton.interactable = false;
    }


    //


    // map select

    public void HyperlaneNext()
    {
        Hyperlane.SetActive(false);
        Warpway.SetActive(true);
        MapName = "Warpway";
        Debug.Log(MapName);
    }

    public void WarpwayNext()
    {
        Warpway.SetActive(false);
        Oblivion.SetActive(true);
        MapName = "Oblivion";
    }

    public void WarpwayPrev()
    {
        Warpway.SetActive(false);
        Hyperlane.SetActive(true);
        MapName = "Hyperlane";
    }

    public void OblivionNext()
    {
        Oblivion.SetActive(false);
        Random.SetActive(true);
        MapName = "Random";
    }

    public void OblivionPrev()
    {
        Oblivion.SetActive(false);
        Warpway.SetActive(true);
        MapName = "Warpway";
    }

    public void RandomPrev()
    {
        Random.SetActive(false);
        Oblivion.SetActive(true);
        MapName = "Oblivion";
    }

    //
}
