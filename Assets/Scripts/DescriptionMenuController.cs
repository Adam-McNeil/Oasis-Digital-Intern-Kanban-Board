using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DescriptionMenuController : MonoBehaviour
{
    private Vector3 farAway = new Vector3(1000, 1000, 1000);

    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI descriptionText;

    private void Start()
    {
        this.transform.position = farAway;
    }

    public void DisplayTicket(GameObject ticket, Transform camera)
    {
        TicketData ticketData = ticket.GetComponent<TicketData>();
        titleText.text = ticketData.headerData;
        descriptionText.text = ticketData.descriptionData;
        this.transform.position = ticket.transform.position;
        this.transform.LookAt(camera);
        this.transform.Rotate(0, 180, 0);
    }

    public void MoveFarAway()
    {
        this.transform.position = farAway;
    }
}
