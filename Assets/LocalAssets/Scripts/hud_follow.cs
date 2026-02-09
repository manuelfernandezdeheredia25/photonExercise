using UnityEngine;

public class hud_follow : MonoBehaviour
{

    public TankController tankPlayer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = tankPlayer.transform.position + new Vector3(0,0,1);
    }
}
