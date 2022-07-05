using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using TMPro;

public class TicketController : NetworkBehaviour
{
    static List<TicketInfo> ticketList = new List<TicketInfo>();
    [SerializeField] private TMP_InputField inputField;
    [SerializeField] private TextMeshProUGUI textObject;
    private TicketInfo myTicketInfo;

    private void Start()
    {
        inputField.onValueChanged.AddListener(delegate { ChangeText(); });
        myTicketInfo.networkIdentity = GetComponent<NetworkIdentity>();
        ticketList.Add(myTicketInfo);
    }

    private void ChangeText()
    {
        myTicketInfo.text = inputField.text;
        ChangeTextCmd(myTicketInfo.text);
    }

    [Command(requiresAuthority = false)]
    private void ChangeTextCmd(string text)
    {
        Debug.Log("cOmmand called with: " + text);
        ChangeTextRpc(text);
    }

    [ClientRpc]
    private void ChangeTextRpc(string text)
    {
        Debug.Log("Client Rpc called with:" + text);
        textObject.text = text;
    }
}

struct TicketInfo
{
    public GameObject gameObject;
    public NetworkIdentity networkIdentity;
    public string text;
}
