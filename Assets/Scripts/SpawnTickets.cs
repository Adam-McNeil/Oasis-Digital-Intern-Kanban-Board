using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class SpawnTickets : NetworkBehaviour
{
    public GameObject ticketGameObject; //Ticket Prefab that will be spawned
    public Transform spawnPosition;       //Postion where to spawn the prefab 

    private bool canInteract = false;   //Makes sure the user is close enough to spawn the prefab.

    private AudioSource buttonAudio;
    
    public GameObject ticketCreator;

    private void Start() {
        buttonAudio = this.GetComponent<AudioSource>();
    }

    
    void Update() 
    {
        if(canInteract && Input.GetMouseButtonDown(0))
        {
            //SpawnTicketCmd();
        }
    }

    public void SpawnTicketCmd(bool newTicket = false)
    {
        GameObject spawnedTicket = Instantiate(ticketGameObject, spawnPosition.position, ticketGameObject.transform.rotation);
        if (newTicket) {
            spawnedTicket.GetComponent<Animator>().Play("Ticket_Spawn");
            spawnedTicket.GetComponent<Animator>().Play("Ticket_Shrink");
        } 
        else {
            spawnedTicket.GetComponent<Animator>().Play("Ticket_Grow");
        }
        NetworkServer.Spawn(spawnedTicket);
    }

    [Command(requiresAuthority = false)]
    public void CreateTicket() {
        SpawnTicketCmd(true);
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
        SpawnTicketCmd();
        ExitCreatorScreen();
    }

    public void PlayClick(){
        buttonAudio.Play();
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
