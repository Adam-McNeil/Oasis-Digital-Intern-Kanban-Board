using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class SpawnTickets : NetworkBehaviour
{
    public GameObject ticketGameObject; //Ticket Prefab that will be spawned
    public Vector3 spawnPosition;       //Postion where to spawn the prefab 

    private bool canInteract = false;   //MNakes sure the user is close enough to spawn the prefab.

    public void SpawnTicket()
    {
        if (canInteract)
        {
            SpawnTicketCmd();
        }
    }

    [Command(requiresAuthority = false)]
    private void SpawnTicketCmd()
    {
        GameObject spawnedTicket = Instantiate(ticketGameObject, spawnPosition, ticketGameObject.transform.rotation);
        NetworkServer.Spawn(spawnedTicket);
    }

    private void OnTriggerEnter(Collider other) 
    {
        if (other.gameObject.CompareTag("Local Player"))
        {
            canInteract = true;
        }
    }

    private void OnTriggerExit(Collider other) 
    {
        if (other.gameObject.CompareTag("Local Player"))
        {
            canInteract = false;
        }
    }



}
