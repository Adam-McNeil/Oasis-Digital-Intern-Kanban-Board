using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EditTable : MonoBehaviour
{

    public GameObject ticket = null;
    private EditMenuController editMenuControllerScript;
    private TicketData ticketDataScript;
    private string ticketHeader;
    private string ticketDetail;
    private int ticketAssignedTo;
    private int ticketMateral;
    
    private bool objectInTable = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.CompareTag("Ticket") && !objectInTable){
            ticket = other.gameObject;
            ticketDataScript = ticket.GetComponent<TicketData>();
            ticketHeader = ticketDataScript.headerData;
            ticketDetail = ticketDataScript.descriptionData;
            ticketAssignedTo = ticketDataScript.assignedToData;
            ticketMateral = ticketDataScript.materialData;
            

            editMenuControllerScript = GameObject.Find("Edit Menu").GetComponent<EditMenuController>();
            editMenuControllerScript.targetedTicket = ticket;
            editMenuControllerScript.headerInputField.text = ticketHeader;
            editMenuControllerScript.detailInputField.text = ticketDetail;
            editMenuControllerScript.assignedDropDown.value = ticketAssignedTo;
            editMenuControllerScript.colorDropDown.value = ticketMateral;
            objectInTable = true;
        }
    }
    
    private void OnTriggerExit(Collider other) {
        ticket = null;
        ticketDataScript = null;
        ticketHeader = null;
        ticketDetail = null;
        ticketAssignedTo = 0;
        ticketMateral = 0;
        objectInTable = false;
    }

}
