using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EditTable : MonoBehaviour
{

    public GameObject ticket = null;
    private EditMenuController editMCS;
    private TicketData ticketDataScript;
    private string ticketHeader;
    private string ticketDetail;
    private int ticketAssignedTo;
    private int ticketMateral;
    
    private bool objectInTable = false;

    // Start is called before the first frame update
    void Start()
    {
        editMCS = GameObject.Find("Edit Menu").GetComponent<EditMenuController>();
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

            editMCS.currentTicketText.text = "Found";
            editMCS.currentTicketText.color = Color.green;
            
            editMCS.targetedTicket = ticket;
            editMCS.headerInputField.text = ticketHeader;
            editMCS.detailInputField.text = ticketDetail;
            editMCS.assignedDropDown.value = ticketAssignedTo;
            editMCS.colorDropDown.value = ticketMateral;
            objectInTable = true;

        }else if(objectInTable == false){
            editMCS.currentTicketText.text = "Not Found";
            editMCS.currentTicketText.color = Color.red;
        }
    }
    
    private void OnTriggerExit(Collider other) {
        ticket = null;
        ticketDataScript = null;
        ticketHeader = null;
        ticketDetail = null;
        ticketAssignedTo = 0;
        ticketMateral = 0;

        editMCS.targetedTicket = ticket;
        editMCS.headerInputField.text = ticketHeader;
        editMCS.detailInputField.text = ticketDetail;
        editMCS.assignedDropDown.value = ticketAssignedTo;
        editMCS.colorDropDown.value = ticketMateral;

        objectInTable = false;
        editMCS.currentTicketText.text = "Not Found";
        editMCS.currentTicketText.color = Color.red;
    }

}
