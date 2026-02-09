using UnityEngine;
using Photon.Pun;
using TMPro;
using System;

public class Chat : MonoBehaviourPunCallbacks
{
    public TMP_InputField inputFieldChat;

    public GameObject Message;

    public GameObject Content;



    public void SendMessage()
    {
        Debug.Log("Enviando mensaje");
        //Hacemos llamada al servidor para targetear a todos los usuarios
        GetComponent<PhotonView>().RPC("GetMessage", RpcTarget.All,PhotonNetwork.NickName + " : " + inputFieldChat.text);

        inputFieldChat.text = "";
    }

    [PunRPC]
    public void GetMessage(string ReceiveMessage)
    {
        GameObject mens = Instantiate(Message, Vector3.zero, Quaternion.identity, Content.transform);

        mens.GetComponent<Message>().miMensaje.text = ReceiveMessage;
    }

}