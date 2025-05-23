using UnityEngine;
using UnityEngine.UI;   
using Photon.Pun;        
using TMPro;


public class UsernameController : MonoBehaviour
{
[SerializeField] private TMP_InputField usernameInput;

    void Start()
    {
        // Load saved username (optional)
        usernameInput.text = PlayerPrefs.GetString("Username", "");
        OnNameChanged(usernameInput.text);

        // Bind event
        usernameInput.onValueChanged.AddListener(OnNameChanged);
    }

    private void OnNameChanged(string newName)
    {
        // Use Photon nickname or store the value
        PhotonNetwork.NickName = string.IsNullOrWhiteSpace(newName)
            ? "Player" + Random.Range(1000, 9999)
            : newName;

        PlayerPrefs.SetString("Username", PhotonNetwork.NickName);
    }
}
