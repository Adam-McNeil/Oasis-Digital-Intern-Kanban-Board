using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AllTickets : MonoBehaviour
{
    public bool zero = false;
    public bool one = false;
    public bool two = false;
    public bool three = false;
    public GameObject[] tickets;
    int a = 0;
    private Color orginalColor;
    RaycastHit hit;
    public TMP_Dropdown dropdownUsers;
    
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(LateStart(.01f));
    }

    private void Update() {

        //VV For Testing VV
        if(zero){
            zero = false;
            GetAllTickets();
            GetCertainTickets(0);
        }
        if(one){
            one = false;
            GetAllTickets();
            GetCertainTickets(1);
        }
        if(two){
            two = false;
            GetAllTickets();
            GetCertainTickets(2);
        }
        if(three){
            three = false;
            GetAllTickets();
            GetCertainTickets(3);
        }


    }
 
    IEnumerator LateStart(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        GetAllTickets();
    }

    private void GetAllTickets(){
        tickets = null;
        tickets = GameObject.FindGameObjectsWithTag("Ticket");
        Debug.Log("All Tickets are found");
    }

    public void FindButton(){
        GetCertainTickets(dropdownUsers.value);
    }


    private void GetCertainTickets(int lookedFor){
        a = 0;
        for(int i = 0; i < tickets.Length; i++){
            if(tickets[i].GetComponent<TicketData>().assignedToData == lookedFor){
                Debug.Log("Found: " + a++ + " | At Pos: " + i);
                orginalColor = tickets[i].GetComponent<Renderer>().material.color;
                tickets[i].GetComponent<Renderer>().material.color = Color.red;
                StartCoroutine(TurnBack(10f, tickets[i].GetComponent<Renderer>(), orginalColor));
                //Debug.Log(tickets[i].transform.position);
                //Debug.DrawLine(this.transform.position, tickets[i].transform.position, Color.red, 10f);
            }
        }
    }

    IEnumerator TurnBack(float waitTime, Renderer ticketcolor, Color Orginal)
    {
        yield return new WaitForSeconds(waitTime);
        ticketcolor.material.color = Orginal;
    }

    



}
