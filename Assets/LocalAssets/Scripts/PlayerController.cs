using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerController : MonoBehaviourPunCallbacks
{

    public float speed = 5;

    void Update()
    {
        if (photonView.IsMine)
        {
            float movHorizontal = Input.GetAxis("Horizontal");
            float movVertical = Input.GetAxis("Vertical");

            Vector3 movimiento = new Vector3(movHorizontal, 0, movVertical) * speed * Time.deltaTime;

            transform.Translate(movimiento);
        }
    }
}