using Photon.Pun;
using Photon.Pun.Demo.Asteroids;
using Photon.Pun.Demo.SlotRacer.Utils;
using System.Collections;
using UnityEngine;

public class BulletScript : MonoBehaviour
{

    public int damage;
    public Vector3 target;
    public float speed;
    public string owner;
    private Vector3 movingDirection = Vector3.zero;

    private void Start()
    {
        StartCoroutine(DestroyBullet());
        movingDirection = target;
        movingDirection = movingDirection.normalized;
    }
    // Update is called once per frame
    void Update()
    {

        transform.position += movingDirection * speed * Time.deltaTime;
    }


    IEnumerator DestroyBullet()
    {
        yield return new WaitForSeconds(4);
        Destroy(gameObject);

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent( out TankController tank))
        {
            if (tank.PlayerName == owner)
                return;
            if (PhotonNetwork.IsMasterClient)
            {
                tank.pv.RPC("SetLife", RpcTarget.All, tank.life - damage);
                Destroy(gameObject);
                if (tank.life <= 0)
                {
                    tank.pv.RPC("DiedTo", RpcTarget.All, owner);
                }
            }
        }


        //Destroy(gameObject);
        ContactPoint firstContact = collision.GetContact(0);

        movingDirection = Vector3.Reflect(movingDirection, firstContact.normal);
    }
}
