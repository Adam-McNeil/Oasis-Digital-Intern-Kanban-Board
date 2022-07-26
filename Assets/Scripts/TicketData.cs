using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Mirror;
using System.IO;

public class TicketData : NetworkBehaviour
{
    static public List<NetworkIdentity> networkIdentities = new List<NetworkIdentity>();

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
    [SerializeField] private List<TextMeshPro> ticketHeaderTextBoxes = new List<TextMeshPro>();

    TicketSaveData ticketSaveData = new TicketSaveData();

    void Start()
    {
        foreach (TextMeshPro ticketHeaderText in ticketHeaderTextBoxes) {
            ticketHeaderText.text = headerData;
        }
        networkIdentities.Add(this.gameObject.GetComponent<NetworkIdentity>());
    }

    private void OnDestroy()
    {
        networkIdentities.Remove(this.gameObject.GetComponent<NetworkIdentity>());
        
    }

    private void ChangeHeaderData(string oldValue, string newValue)
    {
        headerData = newValue;
        foreach (TextMeshPro ticketHeaderText in ticketHeaderTextBoxes) {
            ticketHeaderText.text = headerData;
        }
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

    public string Save()
    {
        ticketSaveData.position = this.transform.position;                          
        ticketSaveData.rotation = this.transform.rotation;
        ticketSaveData.scale = this.transform.localScale;
        ticketSaveData.constraints = this.GetComponent<Rigidbody>().constraints;
        ticketSaveData.headerText = headerData;
        ticketSaveData.descriptionText = descriptionData;
        ticketSaveData.assignedToNumber = assignedToData;
        ticketSaveData.materialNumber = materialData;
        string json = JsonUtility.ToJson(ticketSaveData);
        return json;
    }

    public void Load(string jsonString)
    {
        TicketSaveData loadedTicketSaveData = JsonUtility.FromJson<TicketSaveData>(jsonString);

        this.transform.position = loadedTicketSaveData.position;
        this.transform.rotation = loadedTicketSaveData.rotation;
        this.transform.localScale = loadedTicketSaveData.scale;
        this.GetComponent<Rigidbody>().constraints = loadedTicketSaveData.constraints;

        headerData = loadedTicketSaveData.headerText;
        foreach (TextMeshPro ticketHeaderText in ticketHeaderTextBoxes) {
            ticketHeaderText.text = headerData;
        }

        descriptionData = loadedTicketSaveData.descriptionText;
        assignedToData = loadedTicketSaveData.assignedToNumber;

        materialData = loadedTicketSaveData.materialNumber;
        GetComponent<Renderer>().material = materials[materialData];
    }

    struct TicketSaveData
    {
        public Vector3 position;
        public Quaternion rotation;
        public Vector3 scale;
        public RigidbodyConstraints constraints;
        public string headerText;
        public string descriptionText;
        public int assignedToNumber;
        public int materialNumber;

        public void print()
        {
            Debug.Log(position);
            Debug.Log(rotation);
            Debug.Log(scale);
            Debug.Log(constraints);
            Debug.Log(headerText);
            Debug.Log(descriptionText);
            Debug.Log(assignedToNumber);
            Debug.Log(materialNumber);
        }

    }
}
