using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviourPunCallbacks
{
    public TMP_InputField usernameInput;

    public TMP_Text buttonText;

    public TankSelector tankSelector;

    public void OnClickConnect()
    {
        if (usernameInput.text.Length >= 1)
        {
            PhotonNetwork.NickName = usernameInput.text;

            //Seteamos en el playerprefs llamado "PlayerName" nuestro texto
            PlayerPrefs.SetString("PlayerName", usernameInput.text);
            PlayerPrefs.SetString("TankType", tankSelector.selectedTank.TankPrefab.name);
            buttonText.text = "Conectando al servidor...";
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    public override void OnConnectedToMaster()
    {
        SceneManager.LoadScene("testTanques");
    }

    public void Update()
    {
        if (usernameInput.isFocused == true && Input.GetKeyDown(KeyCode.Return))
        {
            OnClickConnect();
        }
    }
}