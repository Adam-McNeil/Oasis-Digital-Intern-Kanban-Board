using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Mirror;

public class AllTickets : NetworkBehaviour
{
    public bool zero = false;
    public bool one = false;
    public bool two = false;
    public bool three = false;
    public GameObject[] tickets;
    //int a = 0;
    private Color orginalColor;
    public TMP_Dropdown dropdownUsers;

    public void FindButton(){
        HighlightTickets(dropdownUsers.value);
    }

    public void UnFindButton()
    {
        UnhighlightTickets(dropdownUsers.value);
    }

    private void HighlightTickets(int lookedFor)
    {
        foreach (NetworkIdentity networkIdentity in TicketData.networkIdentities)
        {
            if (networkIdentity.gameObject.GetComponent<TicketData>().assignedToData == lookedFor)
            {
                networkIdentity.gameObject.GetComponent<OutlineNetworkController>().UpdateOutlineCmd(true, true);
            }
        }
    }

    private void UnhighlightTickets(int lookedFor)
    {
        foreach (NetworkIdentity networkIdentity in TicketData.networkIdentities)
        {
            if (networkIdentity.gameObject.GetComponent<TicketData>().assignedToData == lookedFor)
            {
                networkIdentity.gameObject.GetComponent<OutlineNetworkController>().UpdateOutlineCmd(false, true);
            }
        }
    }

    IEnumerator TurnBack(float waitTime, Renderer ticketcolor, Color Orginal)
    {
        yield return new WaitForSeconds(waitTime);
        ticketcolor.material.color = Orginal;
    }
}
