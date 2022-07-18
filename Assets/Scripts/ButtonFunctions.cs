using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ButtonFunctions : MonoBehaviour
{
    public void Exit()
    {
        Debug.Log("Game Exited");
        Application.Quit();
    }

    public void ExitServer()
    {
        Mirror.NetworkManager networkMangaer = GameObject.Find("Network Manager").GetComponent<Mirror.NetworkManager>();
        networkMangaer.StopClient();
        networkMangaer.StopHost();
    }
    
    private TicketData ticketDataScript;
    private EditTable editTableScript;
    private EditMenuController eMCS;
    private PlayerController playerControllerScript;
    

    [Header("Material for EditMenu")]
    public Material gray;
    public Material red;
    public Material blue;
    public Material green;
    public Material yellow;
    

    public void SubmitEditChanges()
    {
        eMCS = GameObject.Find("Edit Menu").GetComponent<EditMenuController>();
        GameObject ticket = eMCS.targetedTicket;
        if(ticket != null){
            ticketDataScript = ticket.GetComponent<TicketData>();

            ticketDataScript.ticketHeaderObject.GetComponent<TextMeshPro>().text = eMCS.headerInputField.text;
            ticketDataScript.headerData = eMCS.headerInputField.text;
            ticketDataScript.descriptionData = eMCS.detailInputField.text;
            ticketDataScript.assignedToData = eMCS.assignedDropDown.value;
            ticketDataScript.materialData = eMCS.colorDropDown.value;

            string colorPicked = eMCS.colorDropDown.options[eMCS.colorDropDown.value].text;
            if(colorPicked == "Gray"){
                ticket.GetComponent<Renderer>().material = gray;
            }else if(colorPicked == "Red"){
                ticket.GetComponent<Renderer>().material = red;
            }else if(colorPicked == "Blue"){
                ticket.GetComponent<Renderer>().material = blue;
            }else if(colorPicked == "Green"){
                ticket.GetComponent<Renderer>().material = green;
            }else if(colorPicked == "Yellow"){
                ticket.GetComponent<Renderer>().material = yellow;
            }

            PlayerController.isEditing = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
        
    }
}
