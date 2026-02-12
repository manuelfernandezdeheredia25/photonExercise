
using Photon.Pun;
using Photon.Pun.Demo.PunBasics;
using Photon.Realtime;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class TankControllerBot : TankController
{

    public Launcher launcher;
    public NavMeshAgent agent;


    // Update is called once per frame
    public new void Update()
    {
        Debug.Log("Updating bot");
        if (PhotonNetwork.IsMasterClient && controlEnabled)
        {
            Debug.Log("I am master");

            
           
           
            agent.SetDestination(launcher.GetRandomSpawnPoint().position);
            
            
           
            body.transform.forward = Vector3.RotateTowards(body.transform.forward, agent.velocity, 10 * Time.deltaTime, 10 * Time.deltaTime);

            rotor.LookAt(Vector3.zero);

            if (CheckClearSight() && CooldownOff)
                Debug.Log("Shooting");
                HandleShooting(rotor.transform.forward);
        }


    }


    public bool CheckClearSight()
    {

        if (Physics.Raycast(cannonTip.position,rotor.forward,out RaycastHit hitInfo,500f)){
            if (hitInfo.transform.TryGetComponent<TankController>(out TankController enemy))
            {
                Debug.Log("saw " + enemy.PlayerName);
            }
        }

        return false;
    }

}
