using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using TMPro;

public class TicketController : NetworkBehaviour
{
    //static List<TicketInfo> ticketList = new List<TicketInfo>();
    //private TicketInfo myTicketInfo;
    private float speed = 1;
    private float maxSpeed = 5;
    [SerializeField] private TMP_InputField inputField;
    [SerializeField] private TextMeshProUGUI textObject;
    private TicketTrigger ticketTriggerScript;
    private Rigidbody body;

    private float xInput;
    private float zInput;
    [SyncVar(hook = nameof (ChangeText))]
    private string textSyncVar;

    private bool shouldDoMovement = true;

    //[SyncVar(hook = nameof (ChangePosition))]
    //private Vector3 positionSyncVar;

    private void Start()
    {
        body = GetComponent<Rigidbody>();
        ticketTriggerScript = GetComponentInChildren<TicketTrigger>();
        inputField.onValueChanged.AddListener(delegate { OnChangedInputField(); });
        //myTicketInfo.networkIdentity = GetComponent<NetworkIdentity>();
        //ticketList.Add(myTicketInfo);
    }

    private void Update()
    {
        xInput = Input.GetAxisRaw("Horizontal");
        zInput = Input.GetAxisRaw("Vertical");
    }

    private void FixedUpdate()
    {
        if (shouldDoMovement && ticketTriggerScript.beingEdited)
        {
            Vector3 force = new Vector3(xInput, 0, zInput).normalized * speed;
            if (force.magnitude > 0)
            {
                AddForceCmd(force);
            }
           
        }
        Debug.Log(transform.position);
    }

    [Command(requiresAuthority = false)]
    private void AddForceCmd(Vector3 force)
    {
        body.AddForce(force, ForceMode.Impulse);
        if (body.velocity.magnitude > maxSpeed)
        {
            body.velocity = body.velocity.normalized * maxSpeed;
        }
    }
    
    public void SetShouldDoMovement(bool value)
    {
        shouldDoMovement = value;
    }


    #region SyncText
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
    #endregion
}

/*
struct TicketInfo
{
    public GameObject gameObject;
    public NetworkIdentity networkIdentity;
    public string text;
}
*/
