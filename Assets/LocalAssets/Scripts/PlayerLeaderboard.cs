using Photon.Pun;

using Photon.Realtime;
using System.Collections.Generic;

using UnityEngine;

public class PlayerLeaderboard : MonoBehaviourPunCallbacks
{

    public GameObject leaderboardElementPrefab;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public Dictionary<string, int[]> leaderboardData = new Dictionary<string, int[]>();



    public override void OnJoinedRoom()
    {
        photonView.RPC("JoinLeaderBoard",RpcTarget.MasterClient,PhotonNetwork.NickName);
 
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log("lb sees pplayer " + newPlayer.NickName);

    }

    [PunRPC]
    public void JoinLeaderBoard(string player)
    {
        int[] array = { 0, 0, 0};
        leaderboardData[player] = array;
        photonView.RPC("UpdateLeaderboardUI", RpcTarget.All, leaderboardData);

    }

    [PunRPC]
    public void UpdateLeaderboardUI(Dictionary<string, int[]> leaderboardData)
    {
        this.leaderboardData = leaderboardData;
        Debug.Log("updating table...");
        // first destroy all children
        foreach (UILeaderboardElement element in GetComponentsInChildren<UILeaderboardElement>()) {
         
            Destroy(element.gameObject);
        }
       
        //recreate the ui elements with updated values
        foreach (string player in leaderboardData.Keys)
        {
            GameObject uiRow = Instantiate(leaderboardElementPrefab);
            uiRow.GetComponent<UILeaderboardElement>().UpdateData(player, leaderboardData[player]);
            uiRow.transform.parent = transform;
        }
       
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            leaderboardData.Remove(otherPlayer.NickName);
            photonView.RPC("UpdateLeaderboardUI", RpcTarget.All, leaderboardData);

        }
    }


}
