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

    public void OnClickConnect()
    {
        if (usernameInput.text.Length >= 1)
        {
            PhotonNetwork.NickName = usernameInput.text;

            // setearemos en el player pref llamado "PlayerName" nuestro texto
            PlayerPrefs.SetString("PlayerName", usernameInput.text);
            buttonText.text = "Conectando al servidor...";
            PhotonNetwork.ConnectUsingSettings();
        }

    }

    public override void OnConnectedToMaster()
    {
        SceneManager.LoadScene("SampleScene");
    }
}