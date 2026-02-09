
using Photon.Pun;
using Photon.Pun.Demo.PunBasics;
using Photon.Realtime;
using System;
using System.Collections;
using UnityEngine;

public class TankControllerBot : TankController
{



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

            Vector3 movimiento = new Vector3(movHorizontal, 0, movVertical) * speed;

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

}
