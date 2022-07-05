using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnTickets : MonoBehaviour
{
    public GameObject ticketGameObject; //Ticket Prefab that will be spawned
    public Vector3 spawnPosition;       //Postion where to spawn the prefab 

    private bool canInteract = false;   //MNakes sure the user is close enough to spawn the prefab.

    // Update is called once per frame
    void Update() 
    {
        if(canInteract && Input.GetKeyDown("space")){
            Instantiate(ticketGameObject, spawnPosition, ticketGameObject.transform.rotation);
        }
    }

    private void OnTriggerEnter(Collider other) 
    {
        //Debug.Log("Player Entered Area");
        canInteract = true;
    }

    private void OnTriggerExit(Collider other) 
    {
        //Debug.Log("Player Exited Area");
        canInteract = false;
    }



}
