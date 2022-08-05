using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Mirror;

public class EditTable : NetworkBehaviour
{
    static public EditTable instance = null;

    public EditMenuController eMCS;

    // ticket variables of the selected tickets;
    public GameObject ticket = null;
    private TicketData ticketDataScript;
    
    private void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
        eMCS = GameObject.Find("Edit Menu").GetComponent<EditMenuController>();
    }

    public void OnStartEdit(GameObject ticketSelected)
    {
        ticket = ticketSelected;
        ticketDataScript = ticketSelected.GetComponent<TicketData>();
        eMCS.activeEditTable = this.gameObject;
        eMCS.headerInputField.SetTextWithoutNotify(ticketDataScript.headerData);
        eMCS.detailInputField.SetTextWithoutNotify(ticketDataScript.descriptionData);
        eMCS.assignedDropDown.SetValueWithoutNotify(ticketDataScript.assignedToData);
        eMCS.colorDropDown.SetValueWithoutNotify(ticketDataScript.materialData);
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
    }
}
