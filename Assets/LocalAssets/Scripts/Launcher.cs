using UnityEngine;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Photon.Realtime;

public class Launcher : MonoBehaviourPunCallbacks
{
    //Creamos una variable pública para poder instanciar al jugador a través de su componente PhotonView
    public PhotonView botPrefab;
    
    //Variable pública para poder utilizar el punto de Spawn en el que crear cada objeto jugador.
    public Transform[] spawnPoints;

    public PlayerLeaderboard leaderboard;

    private List<GameObject> bots = new List<GameObject>();

    public GameObject botPanel;
    public TMP_Text botCountLabel;
    
    void Start()
    {
        //Inicia el servidor al empezar la escena.
        // PhotonNetwork.ConnectUsingSettings();

        // OnJoinedRoom();

        PhotonNetwork.JoinRandomOrCreateRoom(expectedMaxPlayers: 4);
    }

    /*
    public override void OnConnectedToMaster()
    {
        Debug.Log("Conexión al master realizada");

        //Esto nos unirá a una sesión o creará una si no existe ninguna.
        PhotonNetwork.JoinRandomOrCreateRoom();
    }
    */

    public override void OnJoinedRoom()
    {
        int sp_index = Random.Range(0, spawnPoints.Length);
        //Creamos la instancia del jugador en el punto de spawn
        GameObject player = PhotonNetwork.Instantiate(PlayerPrefs.GetString("TankType"), spawnPoints[sp_index].position, spawnPoints[sp_index].rotation);
        //Con esto hariamos una llamada al servidor para darle el nombre que guardamos en playerPrefs y llamamos al resto de jugadores para que actualicen su info
        player.GetComponent<PhotonView>().RPC("SetNameText", RpcTarget.AllBuffered, PlayerPrefs.GetString("PlayerName"));
        player.GetComponent<TankController>().OnDied += OnPlayerDead;
        
        if (PhotonNetwork.IsMasterClient) {
            botPanel.SetActive(true);
        }
        Debug.Log("finished player join");
    }

    public Transform GetRandomSpawnPoint()
    {
        int sp_index = Random.Range(0, spawnPoints.Length);
        return spawnPoints[sp_index];
    }
    public void CreateNewBot()
    {
        Transform spawn = GetRandomSpawnPoint();
        GameObject bot = PhotonNetwork.Instantiate(botPrefab.name,spawn.position , spawn.rotation);
        bots.Add(bot);
       
        bot.GetComponent<PhotonView>().RPC("SetNameText", RpcTarget.AllBuffered, "TankBot_0" + bots.Count.ToString());
        bot.GetComponent<TankController>().OnDied += OnPlayerDead;
        bot.GetComponent<TankControllerBot>().launcher = this;

        leaderboard.photonView.RPC("JoinLeaderBoard", RpcTarget.MasterClient, "TankBot_0" + bots.Count.ToString());
    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            botPanel.SetActive(true);
        }
    }

    public void OnPlayerDead(TankController playerDead, string killer)
    {

       
        StartCoroutine(DelayedSpawn(playerDead,5));
        //update leaderboard por dead player

        photonView.RPC("SetPoints", RpcTarget.MasterClient, playerDead.PlayerName, killer);
    }

    [PunRPC]
    public void SetPoints(string playerDead, string killer)
    {

        Debug.Log("adding scores to lead data");
        leaderboard.leaderboardData[playerDead][2] += 1;
        
        leaderboard.leaderboardData[killer][0] += 75;
        leaderboard.leaderboardData[killer][1] += 1;
        if (leaderboard.leaderboardData[playerDead][0] >= 25 )
        {
            leaderboard.leaderboardData[playerDead][0] -= 25;
            leaderboard.leaderboardData[killer][0] += 25;
        }
        
        leaderboard.photonView.RPC("UpdateLeaderboardUI", RpcTarget.All, leaderboard.leaderboardData);
    }

  
    IEnumerator DelayedSpawn(TankController player, int delay)
    {

        yield return new WaitForSeconds(delay);
        Debug.Log("respawing");
        int SpawnPointIndex = Random.Range(0, spawnPoints.Length);
        player.transform.position = spawnPoints[SpawnPointIndex].position;
        player.controlEnabled = true;
        player.pv.RPC("SetLife",RpcTarget.All, player.maxLife);
        // make invincible for some seconds?
    }

}