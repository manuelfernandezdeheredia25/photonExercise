using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;

public class PlayerUsername : MonoBehaviourPunCallbacks
{
    public TMP_Text playerUsername;

    [PunRPC]
    public void SetNameText(string name)
    {
        //Con este método le pasaremos un nombre y se cambiará el nombre del prefab
        playerUsername.text = name;
    }
}