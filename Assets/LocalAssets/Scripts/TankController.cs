
using Photon.Pun;
using Photon.Pun.Demo.PunBasics;
using Photon.Realtime;
using System;
using System.Collections;
using UnityEngine;

public class TankController : MonoBehaviourPunCallbacks
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public float speed = 5;
    public float maxLife = 4;
    public float damage = 1;

    public Transform rotor;
    public Transform body;
    public Transform cannonTip;
    public Transform life_hud;

    public GameObject bulletPrefab;

    public float life;
    public bool controlEnabled = true;

    private float startTime = 0.0f;

    public string PlayerName{ 
        get {
            return gameObject.GetComponent<PlayerUsername>().playerUsername.text;
        }
    }

    [HideInInspector]
    public PhotonView pv;

    public event Action<TankController,string> OnDied;

    [PunRPC]
    public void SetLife(float value)
    {
        life = value;
        float percentLife = life / maxLife;
        Mathf.Max(life, 0);
        life_hud.localScale = new Vector3(percentLife * 2.5f, life_hud.localScale.y, life_hud.localScale.z);
        life_hud.localPosition = new Vector3((percentLife * 2.5f - 2.5f) * .5f, life_hud.localPosition.y, life_hud.localPosition.z);

    }

    public bool CooldownOff
    {
        get {
            return Time.time - startTime > 1f;
        }
    }
    void Start()
    {
        life = maxLife;
        pv = GetComponent<PhotonView>();
    }

    // Update is called once per frame
    void Update()
    {
        if (photonView.IsMine && controlEnabled)
        {
            float movHorizontal = Input.GetAxis("Horizontal");
            float movVertical = Input.GetAxis("Vertical");

            Vector3 movimiento = new Vector3(movHorizontal, 0, movVertical).normalized * speed;

            //transform.Translate(movimiento);
            //GetComponent<Rigidbody>().AddForce(movimiento, ForceMode.VelocityChange);
            GetComponent<Rigidbody>().linearVelocity = movimiento;
            if (movimiento == Vector3.zero)
            {
                GetComponent<Rigidbody>().linearVelocity = Vector3.zero; 
            }

            Vector3 mouseWorlPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mouseWorlPos = new Vector3(mouseWorlPos.x, rotor.position.y, mouseWorlPos.z);
            body.transform.forward = Vector3.RotateTowards(body.transform.forward, new Vector3(movHorizontal, 0, movVertical), 10 * Time.deltaTime, 10 * Time.deltaTime);

            rotor.LookAt(mouseWorlPos);



            if (Input.GetMouseButton(0) && CooldownOff)
                HandleShooting(rotor.transform.forward);
        }

       
    }

    public void startCooldown()
    {
        startTime = Time.time;
    }

    protected void HandleShooting(Vector3 shootDirection )
    {

        pv.RPC("ShootBullet", RpcTarget.All, shootDirection);       
        
    }

    [PunRPC]
    public void ShootBullet(Vector3 bulletInfo)
    {
        GameObject bullet = Instantiate(bulletPrefab);
        bullet.transform.position = cannonTip.position;
        bullet.GetComponent<BulletScript>().target = bulletInfo;
        bullet.GetComponent<BulletScript>().owner = PlayerName;
        startCooldown();
    }

    [PunRPC]
    public void DiedTo(string killerNick)
    {
        // death animation?
        Debug.Log("in diedTo of " + PlayerName);
        // hide the dead
        controlEnabled = false;
        transform.position += new Vector3(0, -10, 0);
        // add points to killer
        // TODO
        // reset life of dead
        OnDied?.Invoke(this,killerNick);

    }



    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        
        GetComponent<PhotonView>().RPC("SetLife", newPlayer, life);
    }



    public void OnCollisionEnter(Collision collision)
    {


        //if (PhotonNetwork.IsMasterClient &&
        //    collision.gameObject.TryGetComponent(out BulletScript bullet) )
        //{
        //    if (bullet.owner == PlayerName) return;

        //    pv.RPC("SetLife", RpcTarget.All, life - bullet.damage);
        //    if (life <= 0)
        //    {
        //        pv.RPC("DiedTo", RpcTarget.All,bullet.owner);   
        //    }
        //}
    }
}
