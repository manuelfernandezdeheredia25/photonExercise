using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class Launcher : MonoBehaviourPunCallbacks
{

    //Variable publica par instanciar players con PhotonView
    public PhotonView playerPrefab;

    public Transform spawnPoint;

    void Start()
    {
        // Iniciamos el servidor en start con default setting
        //PhotonNetwork.ConnectUsingSettings();

        //OnJoinedRoom();

        PhotonNetwork.JoinRandomOrCreateRoom();
    }

    //public override void OnConnectedToMaster()
    //{
    //    Debug.Log("Conexion al master realizada");
    //    PhotonNetwork.JoinRandomOrCreateRoom();
    //}

    public override void OnJoinedRoom()
    {
        //Creamos la instancia del jugador en el transform de spawn
        //base.OnJoinedRoom();
        PhotonNetwork.Instantiate(playerPrefab.name, spawnPoint.position, spawnPoint.rotation);


        // llama al servidor para darle un nombre que guardamos en el playerpref y llamamos al resto de jugadores para que actualicen sus instancias del juego.
        playerPrefab.GetComponent<PhotonView>().RPC("SetNameText", RpcTarget.AllBuffered, PlayerPrefs.GetString("PlayerName"));


    }

}
