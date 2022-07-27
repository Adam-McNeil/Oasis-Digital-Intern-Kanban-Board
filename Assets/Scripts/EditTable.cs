using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Mirror;

public class EditTable : NetworkBehaviour
{

    public EditMenuController eMCS;

    // ticket variables of the selected tickets;
    public GameObject ticket = null;
    private TicketData ticketDataScript;
    
    private bool objectInTable = false;


    private void Start()
    {
        eMCS = GameObject.Find("Edit Menu").GetComponent<EditMenuController>();
    }

    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.CompareTag("Ticket") && !objectInTable){
            ticket = other.gameObject;
            ticketDataScript = ticket.GetComponent<TicketData>();

            objectInTable = true;
        }
    }
    
    private void OnTriggerExit(Collider other) {
        ticket = null;
        ticketDataScript = null;
        objectInTable = false;
    }

    public void OnStartEdit()
    {
        eMCS.activeEditTable = this.gameObject;
        if (ticket != null)
        {
            eMCS.headerInputField.SetTextWithoutNotify(ticketDataScript.headerData);
            eMCS.detailInputField.SetTextWithoutNotify(ticketDataScript.descriptionData);
            eMCS.assignedDropDown.SetValueWithoutNotify(ticketDataScript.assignedToData);
            eMCS.colorDropDown.SetValueWithoutNotify(ticketDataScript.materialData);
        }
    }

    public void SubmitEditChanges()
    {
        if (ticket != null)
        {
            ticketDataScript = ticket.GetComponent<TicketData>();
            ticketDataScript.SubmitEditChangesCmd(eMCS.headerInputField.text, eMCS.detailInputField.text, eMCS.assignedDropDown.value, eMCS.assignedDropDown.options[eMCS.assignedDropDown.value].text, eMCS.colorDropDown.value);
            PlayerController.isEditing = false;
            Cursor.lockState = CursorLockMode.Locked;
        }

        //ticketDataScript.ticketHeaderObject.GetComponent<TextMeshPro>().text = eMCS.headerInputField.text;
        //ticketDataScript.headerData = eMCS.headerInputField.text;
        //ticketDataScript.descriptionData = eMCS.detailInputField.text;
        //ticketDataScript.assignedToData = eMCS.assignedDropDown.value;
        //ticketDataScript.materialData = eMCS.colorDropDown.value;

    }


}
