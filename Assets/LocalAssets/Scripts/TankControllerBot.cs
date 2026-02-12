
using Photon.Pun;
using Photon.Pun.Demo.PunBasics;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class TankControllerBot : TankController
{

    public Launcher launcher;
    public NavMeshAgent agent;
    
    // Update is called once per frame

    public new void Start()
    {
        base.Start();

        
    }
    public new void Update()
    {

        if (PhotonNetwork.IsMasterClient && controlEnabled)
        {
            if (agent.hasPath == false)
            {
                agent.SetDestination(launcher.GetRandomSpawnPoint().position);
            }
           
            body.transform.forward = Vector3.RotateTowards(body.transform.forward, agent.velocity, 10 * Time.deltaTime, 10 * Time.deltaTime);

            if (launcher.tanks.Count > 0)
            {
                
                GameObject closestTank = launcher.tanks.Where(x => x != gameObject).Aggregate<GameObject>((acc, x) => 
                     (Vector3.Distance(acc.transform.position,transform.position) < Vector3.Distance(x.transform.position, transform.position)) ? acc : x   
                );
                Debug.Log(closestTank.name);
                rotor.LookAt(closestTank.transform.position);
            }
            

            if (CheckClearSight() && CooldownOff)
            {
                Debug.Log("Shooting");
                HandleShooting(rotor.transform.forward);
            }
        }


    }


    public bool CheckClearSight()
    {

        if (Physics.Raycast(cannonTip.position,rotor.forward,out RaycastHit hitInfo,500f)){
            if (hitInfo.transform.TryGetComponent<TankController>(out TankController enemy))
            {
                
                return true;
            }
        }

        return false;
    }
}
