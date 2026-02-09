
using UnityEngine;
using TMPro;


public class UILeaderboardElement : MonoBehaviour
{
    public string playerName;
    public int points, kills , deaths;

    public TMP_Text uiName, uiPoints, uiKillls, uiDeaths;

    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void SetName(string name)
    {
        playerName = name;
        uiName.text = name;
    }

  
    public void SetPoints(int points)
    {
        this.points = points;
        uiPoints.text = points.ToString();
    }
   
    public void SetKills(int kills)
    {
        this.kills = kills;
        uiKillls.text = kills.ToString();
    }

    public void SetDeaths(int deaths)
    {
        this.deaths = deaths;
        uiDeaths.text = deaths.ToString();
    }

    public void UpdateData(string player, int[] ints)
    {
        SetName(player);
        SetPoints(ints[0]);
        SetKills(ints[1]);
        SetDeaths(ints[2]);
    }
}
