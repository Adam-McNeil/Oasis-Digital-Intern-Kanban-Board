using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllTickets : MonoBehaviour
{
    public bool nonSelcted = false;
    public bool aden = false;
    public bool adam = false;
    public bool manny = false;
    public GameObject[] tickets;
    int a = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(LateStart(.01f));
    }

    private void Update() {
        
        if(nonSelcted){
            nonSelcted = false;
            int a = 0;
            GetAllTickets();
            GetCertainTickets(0);
        }

        if(adam){
            adam = false;
            int a = 0;
            GetAllTickets();
            GetCertainTickets(1);
        }
        if(aden){
            aden = false;
            int a = 0;
            GetAllTickets();
            GetCertainTickets(2);
        }
        if(manny){
            manny = false;
            int a = 0;
            GetAllTickets();
            GetCertainTickets(3);
        }


    }
 
    IEnumerator LateStart(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        GetAllTickets();
    }

    void GetAllTickets(){
        tickets = null;
        tickets = GameObject.FindGameObjectsWithTag("Ticket");
        Debug.Log("All Tickets are found ");
    }

    void GetCertainTickets(int lookedFor){
        for(int i = 0; i < tickets.Length; i++){
            if(tickets[i].GetComponent<TicketData>().assignedToData == lookedFor){
                    Debug.Log("Found: " + a++ + " | At Pos: " + i);
            }
        }
    }



}
