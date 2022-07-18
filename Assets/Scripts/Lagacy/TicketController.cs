using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using TMPro;

public class TicketController : NetworkBehaviour
{
    [SerializeField] private TMP_InputField inputField;
    [SerializeField] private TextMeshProUGUI textObject;

    [SyncVar(hook = nameof (ChangeText))]
    private string textSyncVar;


    private void Start()
    {
        if (isClientOnly)
        {
            //Destroy(GetComponent<Rigidbody>());
        }
    }

    #region Movement



    #endregion

    #region Delete

    [Command(requiresAuthority = false)]
    public void DeleteTicketCmd()
    {
        //NetworkServer.Destroy(gameObject);
    }

    #endregion

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

