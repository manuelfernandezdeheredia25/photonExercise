using Photon.Pun;
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
        }
        Destroy(gameObject);
    }
}
