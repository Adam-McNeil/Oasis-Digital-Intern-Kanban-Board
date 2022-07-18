using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System.IO;

public class ServerLoaderController : NetworkBehaviour
{
    [SerializeField] private GameObject ticketPrefab;
    [SerializeField] private GameObject columnPrefab;

    private char key;

    private void Start()
    {
        if (File.Exists(Application.dataPath + "\\PutFileInHereToLoadIt\\" + "saveFile.json"))
        {
            Debug.Log("file was loaded");
            string[] jsonStringList = File.ReadAllLines(Application.dataPath + "\\PutFileInHereToLoadIt\\" + "saveFile.json");
            foreach (string jsonString in jsonStringList)
            {
                key = jsonString[0];
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
                        GameObject spawnedColumn= Instantiate(columnPrefab);
                        spawnedColumn.GetComponent<EditColumn>().Load(jsonStringCopy);
                        NetworkServer.Spawn(spawnedColumn);
                        break;

                    default:
                        break;
                }
            }
        }
        else
        {
            Debug.Log("file was not loaded");

        }
    }
}
