using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnTickets : MonoBehaviour
{
    public GameObject ticketGameobject;
    public Vector3 spawnPosition;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay(Collider other) {
       Debug.Log("Player In Area");
        
        if(Input.GetKeyDown("space")){
            Debug.Log("Ticket Spawned");
        }
         
    }




}
