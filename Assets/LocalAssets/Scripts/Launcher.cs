using UnityEngine;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Photon.Realtime;
using System.Linq;
using UnityEngine.SceneManagement;

public class Launcher : MonoBehaviourPunCallbacks
{

    public int winningPoints = 3000;
    public GameObject winPanel;
    //Creamos una variable pública para poder instanciar al jugador a través de su componente PhotonView
    public PhotonView botPrefab;
    
    //Variable pública para poder utilizar el punto de Spawn en el que crear cada objeto jugador.
    public Transform[] spawnPoints;

    public PlayerLeaderboard leaderboard;
    [HideInInspector]
    public List<GameObject> players = new List<GameObject>();
    private List<GameObject> bots = new List<GameObject>();
    
    public List<GameObject> tanks
    {
        get
        {
            return players.Concat(bots).ToList();
        }
    }

    public GameObject botPanel;
    public TMP_Text botCountLabel;
    
    void Start()
    {
        PhotonNetwork.JoinRandomOrCreateRoom(expectedMaxPlayers: 4);
    }


    public override void OnJoinedRoom()
    {
        int sp_index = Random.Range(0, spawnPoints.Length);
        //Creamos la instancia del jugador en el punto de spawn
        GameObject player = PhotonNetwork.Instantiate(PlayerPrefs.GetString("TankType"), spawnPoints[sp_index].position, spawnPoints[sp_index].rotation);
        //Con esto hariamos una llamada al servidor para darle el nombre que guardamos en playerPrefs y llamamos al resto de jugadores para que actualicen su info
        player.GetComponent<PhotonView>().RPC("SetNameText", RpcTarget.AllBuffered, PlayerPrefs.GetString("PlayerName"));
        player.GetComponent<TankController>().OnDied += OnPlayerDead;
        

        players.Add(player);
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

    public Transform GetClearSpawnPoint()
    {
        foreach (Transform s in spawnPoints)
        {
            //check if any tank is close to spawn point
            if (tanks.Any((x) => Vector3.Distance(x.transform.position, s.position) < 25))
                continue;    
            return s;
        }
        // if not one is clear return a random one(should be impossible for not one being clear)
        return GetRandomSpawnPoint();
    }

    public Transform GetCrowdedSpawnPoint()
    {
        foreach (Transform s in spawnPoints)
        {
            //check if any tank is close to spawn point
            if (tanks.Any((x) => Vector3.Distance(x.transform.position, s.position) < 15))
                return s;
           
        }
        // if not one is clear return a random one(should be impossible for not one being clear)
        return GetRandomSpawnPoint();
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        if (!PhotonNetwork.IsMasterClient)
            return;

        if (bots.Count + PhotonNetwork.CurrentRoom.PlayerCount > 4) {
            RemoveBot();
        }
    }
    public void CreateNewBot()
    {
        if (bots.Count + PhotonNetwork.CurrentRoom.PlayerCount >= 4)
        {
            Debug.LogWarning("Max number of players reached");
            return;
        }

        Transform spawn = GetClearSpawnPoint();
        GameObject bot = PhotonNetwork.Instantiate(botPrefab.name,spawn.position , spawn.rotation);
        bots.Add(bot);
       
        bot.GetComponent<PhotonView>().RPC("SetNameBot", RpcTarget.AllBuffered, "TankBot_0" + bots.Count.ToString());
        bot.GetComponent<TankController>().OnDied += OnPlayerDead;
        bot.GetComponent<TankControllerBot>().launcher = this;
        botCountLabel.text = bots.Count.ToString();
        leaderboard.photonView.RPC("JoinLeaderBoard", RpcTarget.MasterClient, "TankBot_0" + bots.Count.ToString());
    }

    public void RemoveBot()
    {
        PhotonNetwork.Destroy(bots[bots.Count - 1]);
        leaderboard.leaderboardData.Remove(bots[bots.Count - 1].GetComponent<TankController>().PlayerName);
        leaderboard.photonView.RPC("UpdateLeaderboardUI", RpcTarget.All, leaderboard.leaderboardData);
        bots.RemoveAt(bots.Count - 1);
        botCountLabel.text = bots.Count.ToString();
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

        if (leaderboard.leaderboardData[killer][0] >= winningPoints)
        {
            //Change to winning scene
            Debug.Log(killer + " has won the game!");
            photonView.RPC("EndMatch", RpcTarget.All,killer);
        }


        leaderboard.photonView.RPC("UpdateLeaderboardUI", RpcTarget.All, leaderboard.leaderboardData);
    }

    [PunRPC]
    public void EndMatch(string winner)
    {

        winPanel.SetActive(true);
        winPanel.GetComponentInChildren<TMP_Text>().text = winner + " ganó la partida!";
        StartCoroutine(DelayedQuit());

    }

  
    IEnumerator DelayedSpawn(TankController player, int delay)
    {

        yield return new WaitForSeconds(delay);
        Debug.Log("respawing");
        player.transform.position = GetClearSpawnPoint().position;
        player.controlEnabled = true;
        player.pv.RPC("SetLife",RpcTarget.All, player.maxLife);
        // make invincible for some seconds?
    }
    IEnumerator DelayedQuit()
    {
        yield return new WaitForSeconds(10);
        OnQuit();
    }

    public void OnQuit()
    {
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.Disconnect();
        SceneManager.LoadScene("MenuPrincipal");
    }

}