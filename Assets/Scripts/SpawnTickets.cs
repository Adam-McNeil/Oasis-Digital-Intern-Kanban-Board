using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnTickets : MonoBehaviour
{
    public GameObject ticketGameObject;
    public Vector3 spawnPosition;
    private bool canInteract = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(canInteract && Input.GetKeyDown("space")){
            Instantiate(ticketGameObject, spawnPosition, ticketGameObject.transform.rotation);
        }
    }

    private void OnTriggerEnter(Collider other) {
        //Debug.Log("Player Entered Area");
        canInteract = true;
    }

    private void OnTriggerExit(Collider other) {
        //Debug.Log("Player Exited Area");
        canInteract = false;
    }



}
