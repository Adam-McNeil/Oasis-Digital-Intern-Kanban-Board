using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System.IO;
using System.Linq;

public class ServerLoaderController : NetworkBehaviour
{
    [SerializeField] private GameObject ticketPrefab;
    [SerializeField] private GameObject columnPrefab;
    public static string serverJSONString = null;

    private char key;
    List<string> serverJSONTextToList = new List<string>();

    public static string stringOutput;

    private void Start()
    {
        if (serverJSONString == null)
        {
            Debug.Log("severJSONstring was null");
            return;
        }
        if (serverJSONString == "")
        {
            Debug.Log("severJSONstring was \"\"");
            return;
        }
        serverJSONTextToList = serverJSONString.Split("\\n").ToList();
        Debug.Log("---------------------------------------------");
        foreach (string x in serverJSONTextToList)
        {
            Debug.Log(x);
        }
        foreach (string jsonString in serverJSONTextToList)
        {
            if (jsonString == "")
            {
                continue;
            }
            Debug.Log("____________________________________________________");
            key = jsonString[0];
            Debug.Log(key);
            string jsonStringCopy = jsonString.Remove(0, 1);
            switch (key)
            {
                case 'T':
                    Debug.Log("Making Ticket");
                    Debug.Log(jsonStringCopy);
                    GameObject spawnedTicket = Instantiate(ticketPrefab);
                    spawnedTicket.GetComponent<TicketData>().Load(jsonStringCopy);
                    NetworkServer.Spawn(spawnedTicket);
                    break;

                case 'C':
                    Debug.Log("Making Column");
                    Debug.Log(jsonStringCopy);
                    GameObject spawnedColumn = Instantiate(columnPrefab);
                    spawnedColumn.GetComponent<EditColumn>().Load(jsonStringCopy);
                    NetworkServer.Spawn(spawnedColumn);
                    break;

                default:
                    break;
            }
        }
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

    struct ColumnSaveData
    {
        public Vector3 position;
        public Quaternion rotation;
        public string title;

        public void print()
        {
            Debug.Log(position);
            Debug.Log(rotation);
            Debug.Log(title);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown("`"))
        {
            stringOutput = "";
            GameObject[] tickets = GameObject.FindGameObjectsWithTag("Ticket");
            GameObject[] columns = GameObject.FindGameObjectsWithTag("Column");
            foreach (GameObject ticket in tickets)
            {
                TicketData ticketData = ticket.GetComponent<TicketData>();
                string jsonString = ticketData.Save();
                stringOutput += "T" + jsonString + "\\n";
            }
            foreach (GameObject column in columns)
            {
                EditColumn editColumn = column.GetComponent<EditColumn>();
                string jsonString = editColumn.Save();
                stringOutput += "C" + jsonString + "\\n";
            }
            GameObject FBM = GameObject.Find("FirebaseManager");
            FBM.GetComponent<FirebaseManager>().saveJSONCall();
        }
    }
}
