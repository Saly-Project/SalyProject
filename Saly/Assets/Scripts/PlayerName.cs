using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;



public class PlayerName : MonoBehaviour
{
    
    [SerializeField] private TMP_InputField nameInputField = null;
    [SerializeField] private Button continueButton = null;
    public static string DisplayName {get; private set;}
    private const string PlayerPrefsNameKey = "PlayerName";

    void Start()
    {
        SetUpInputField();
    }

    private void SetUpInputField()
    {
        if (!PlayerPrefs.HasKey(PlayerPrefsNameKey)) return;
        string defaultName = PlayerPrefs.GetString(PlayerPrefsNameKey);
        nameInputField.text = defaultName;
        SetPlayerName(defaultName);

    }

    private void SetPlayerName(string defaultName)
    {
        continueButton.interactable = !string.IsNullOrEmpty(name);
    }

    public void SavePlayerName()
    {
        DisplayName = nameInputField.text;
        PlayerPrefs.SetString(PlayerPrefsNameKey, DisplayName);
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
