using Photon.Pun;
using Photon.Pun.Demo.Asteroids;
using Photon.Pun.Demo.SlotRacer.Utils;
using System.Collections;
using UnityEngine;

public class BulletScript : MonoBehaviour
{

    public float damage;
    public Vector3 target;
    public float speed;
    public int bounceCount;
    
    public string owner;
    private Vector3 movingDirection = Vector3.zero;

    private int bounces = 0;
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
        transform.forward = movingDirection;
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
                Debug.Log("bullet hit");
                tank.pv.RPC("SetLife", RpcTarget.All, tank.life - damage);
                if (tank.life <= 0)
                {
                    tank.pv.RPC("DiedTo", RpcTarget.All, owner);
                    Destroy(gameObject);
                    return;
                }
            }
            Destroy(gameObject);
        }

        bounces++;
        if (bounces > bounceCount)
        {
            Destroy(gameObject);
            return;
        }

        ContactPoint firstContact = collision.GetContact(0);
        
        movingDirection = Vector3.Reflect(movingDirection, firstContact.normal);
    }
}
