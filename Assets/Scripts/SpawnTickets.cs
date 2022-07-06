using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnTickets : MonoBehaviour
{
    public GameObject ticketGameObject; //Ticket Prefab that will be spawned
    public Vector3 spawnPosition;       //Postion where to spawn the prefab 

    private bool canInteract = false;   //Makes sure the user is close enough to spawn the prefab.
    
    public GameObject ticketCreator;

    
    void Update() 
    {
        if(canInteract && Input.GetKeyDown("space")){
            DisplayCreatorScreen();
        }
    }

    private PlayerController GetPlayerController() {
        GameObject player = GameObject.FindGameObjectWithTag("Local Player");
        if (player == null) {
            player = GameObject.FindGameObjectWithTag("Nonlocal Player");
        }
        PlayerController controller = player.GetComponent<PlayerController>();
        return controller;
    }

    private void DisplayCreatorScreen() {
        ticketCreator.SetActive(true);
        PlayerController controller = GetPlayerController();
        controller.Pause();
    }

    public void ExitCreatorScreen() {
        ticketCreator.SetActive(false);
        PlayerController controller = GetPlayerController();
        controller.Resume();
    }

    public void CreateTicketObject() {
        Instantiate(ticketGameObject, spawnPosition, ticketGameObject.transform.rotation);
        ExitCreatorScreen();
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
