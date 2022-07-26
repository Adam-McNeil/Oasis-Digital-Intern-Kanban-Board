using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DescriptionMenuController : MonoBehaviour
{
    private Vector3 farAway = new Vector3(1000, 1000, 1000);


    private Vector3 smallScale = new Vector3(.1f, .1f, .1f);
    private Vector3 largeScale = new Vector3(3, 3, 3);
    private Vector3 targetPosition;

    private bool shouldUpdatePositionSlerp;
    private float offset = 5f;
    private float yOffSet = 7;
    private float slerpSpeed = 5;

    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI descriptionText;
    [SerializeField] private TextMeshProUGUI assignedToText;

    private void Start()
    {
        this.transform.position = farAway;
    }

    private void Update()
    {
        this.transform.localScale = Vector3.Slerp(this.transform.localScale, largeScale, slerpSpeed * Time.deltaTime);
        if (shouldUpdatePositionSlerp)
        {
            this.transform.position = Vector3.Slerp(this.transform.position, targetPosition, slerpSpeed * Time.deltaTime);
        }
    }

    public void DisplayTicket(GameObject ticket)
    {
        TicketData ticketData = ticket.GetComponent<TicketData>();
        titleText.text = ticketData.headerData;
        descriptionText.text = ticketData.descriptionData;
        if (ticketData.assignedToData >= getCurrentUsers.users.Count)
        {
            ticketData.assignedToData = 0;
        }
        assignedToText.text = getCurrentUsers.users[ticketData.assignedToData];

        this.transform.position = ticket.transform.position;
        this.transform.rotation = ticket.transform.rotation;
        this.transform.Rotate(90, 0, 0);
        this.transform.localScale = smallScale;

        shouldUpdatePositionSlerp = true;
        targetPosition = ticket.transform.position + ticket.transform.up * offset;
        targetPosition.y = yOffSet;
    }

    public void MoveFarAway()
    {
        this.transform.position = farAway;
        shouldUpdatePositionSlerp = false;
    }
}
