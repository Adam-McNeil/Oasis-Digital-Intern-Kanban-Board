using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using TMPro;

public class TicketController : NetworkBehaviour
{
    //static List<TicketInfo> ticketList = new List<TicketInfo>();
    [SerializeField] private TMP_InputField inputField;
    [SerializeField] private TextMeshProUGUI textObject;
    //private TicketInfo myTicketInfo;

    [SyncVar(hook = nameof(ChangeText))]
    private string textSyncVar;

    private void Start()
    {
        inputField.onValueChanged.AddListener(delegate { OnChangedInputField(); });
        //myTicketInfo.networkIdentity = GetComponent<NetworkIdentity>();
        //ticketList.Add(myTicketInfo);
    }

    private void OnChangedInputField()
    {
        ChangeTextCmd(inputField.text);
    }

    [Command(requiresAuthority = false)]
    private void ChangeTextCmd(string text)
    {
        textSyncVar = text;
    }

    private void ChangeText(string oldText, string newText)
    {
        textObject.text = newText;
    }
}

/*
struct TicketInfo
{
    public GameObject gameObject;
    public NetworkIdentity networkIdentity;
    public string text;
}
*/
