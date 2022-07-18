using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Mirror;

public class TicketData : NetworkBehaviour
{
    public GameObject ticketHeaderObject;
    [SyncVar(hook = nameof(ChangeHeaderData))]
    public string headerData = "Header";
    [SyncVar(hook = nameof(ChangeDescriptionData))]
    public string descriptionData = "N/A";
    [SyncVar(hook = nameof(ChangeAssignedToData))]
    public int assignedToData = 0;
    [SyncVar(hook = nameof(ChangeMaterialData))]
    public int materialData = 0;

    [SerializeField] private List<Material> materials = new List<Material>();

    [SerializeField] private TextMeshPro ticketHeaderText;

    void Start()
    {
        ticketHeaderText.text = headerData;
        
    }

    private void ChangeHeaderData(string oldValue, string newValue)
    {
        headerData = newValue;
        ticketHeaderText.text = headerData;
        Debug.Log("headerData = newValue;");
    }

    private void ChangeDescriptionData(string oldValue, string newValue)
    {
        descriptionData = newValue;
        Debug.Log("descriptionData = newValue;");

    }

    private void ChangeAssignedToData(int oldValue, int newValue)
    {
        assignedToData = newValue;
        Debug.Log("assignedToData = newValue;");

    }

    private void ChangeMaterialData(int oldValue, int newValue)
    {
        materialData = newValue;
        GetComponent<Renderer>().material = materials[materialData];
        Debug.Log("materialData = newValue;");

    }

    [Command(requiresAuthority = false)]
    public void SubmitEditChangesCmd(string newHeader, string newDescription, int newAssignTo, int newMaterial)
    {
        headerData = newHeader;
        descriptionData = newDescription;
        assignedToData = newAssignTo;
        materialData = newMaterial;
    }
}
