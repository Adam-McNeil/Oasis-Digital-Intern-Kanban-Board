using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Mirror;

public class EditColumn : NetworkBehaviour
{
    [SerializeField] private TMP_InputField titleInputField;
    [SyncVar(hook = nameof(ChangeInputField))]
    private string title;

    private void Start()
    {
        titleInputField.onValueChanged.AddListener(delegate { ChangeTitleCommand(titleInputField.text); });
    }

    public void SelectInputField()
    {
        titleInputField.Select();
    }
    public void DeselectInputField()
    {
        titleInputField.Select();
    }

    public void SetIsEditingColumn(bool newValue)
    {
        PlayerController.isEditingColumn = newValue;
    }

    private void ChangeInputField(string oldValue, string newValue)
    {
        title = newValue;
        titleInputField.SetTextWithoutNotify(title);
    }

    [Command(requiresAuthority = false)]
    public void ChangeTitleCommand(string newTitle)
    {
        title = newTitle;
        if (title == "delete")
        {
            OnDeleteColumn();
            SetIsEditingColumn(false);
            NetworkServer.Destroy(this.gameObject);
        }
    }

    [ClientRpc]
    private void OnDeleteColumn()
    {
        SetIsEditingColumn(false);
    }
}
