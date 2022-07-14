using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TicketData : MonoBehaviour
{
    public GameObject ticketHeaderObject;
    public string headerData = "Header";
    public string descriptionData = "N/A";
    public int assignedToData = 0;
    public int materialData = 0;

    private TextMeshProUGUI ticketHeaderText;

    // Start is called before the first frame update
    void Start()
    {
        ticketHeaderObject.GetComponent<TextMeshPro>().text = headerData;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
