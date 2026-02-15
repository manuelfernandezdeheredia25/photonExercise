
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
            if (agent.hasPath == false || agent.isStopped || agent.isPathStale)
            {
                agent.isStopped = false;
                agent.SetDestination(launcher.GetRandomSpawnPoint().position);
            }
           
            body.transform.forward = Vector3.RotateTowards(body.transform.forward, agent.velocity, 10 * Time.deltaTime, 10 * Time.deltaTime);

            if (launcher.tanks.Count > 0)
            {
                var otherTanks = launcher.tanks.Where(x => x != gameObject);
                GameObject closestTank = otherTanks.Aggregate<GameObject>((acc, x) => 
                     (Vector3.Distance(acc.transform.position,transform.position) < Vector3.Distance(x.transform.position, transform.position)) ? acc : x   
                );

                if (Vector3.Distance(closestTank.transform.position,transform.position) < 50)
                {
                    Vector3 dirBetween = transform.position - closestTank.transform.position;
                    agent.SetDestination(closestTank.transform.position + dirBetween.normalized * 50);
                    
                }
                
                rotor.LookAt(closestTank.transform.position);
            }
            

            if (CheckClearSight() && CooldownOff)
            {
                HandleShooting(rotor.transform.forward);
            }
        }


    }
    [PunRPC]
    public override void DiedTo(string killerNick)
    {
        agent.isStopped = true;
        controlEnabled = false;
        agent.Warp( new Vector3(0, -10, 0));
        agent.baseOffset = -20;
        launcher.photonView.RPC("SetPoints", RpcTarget.MasterClient, PlayerName, killerNick);
        StartCoroutine(DelayedBotRespawn());
    }

    private IEnumerator DelayedBotRespawn()
    {
        yield return new WaitForSeconds(5);
        Debug.Log("respawing bot");
        
        transform.position = launcher.GetClearSpawnPoint().position;
        controlEnabled = true;
        agent.baseOffset = 0.5f;
        agent.isStopped = false;
        pv.RPC("SetLife", RpcTarget.All, maxLife);
        // make invincible for some seconds?
    }

    public bool CheckClearSight()
    {

        if (Physics.Raycast(cannonTip.position,rotor.forward,out RaycastHit hitInfo,500f)){
            if (hitInfo.transform.TryGetComponent<TankController>(out _))
            {
                
                return true;
            }
        }

        return false;
    }
}
