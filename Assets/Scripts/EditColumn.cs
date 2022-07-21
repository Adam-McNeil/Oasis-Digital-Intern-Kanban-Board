using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Mirror;
using System.IO;

public class EditColumn : NetworkBehaviour
{
    [SerializeField] private TMP_InputField titleInputField;
    [SyncVar(hook = nameof(ChangeInputField))]
    public string title;

    private void Start()
    {
        titleInputField.onValueChanged.AddListener(delegate { ChangeTitleCommand(titleInputField.text); });
    }


    #region EditColumn
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

#endregion

    #region SaveData

    private ColumnSaveData columnSaveData = new ColumnSaveData();
    string json;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            Save();
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            columnSaveData = JsonUtility.FromJson<ColumnSaveData>(json);
            Load();
            columnSaveData.print();
        }
    }

    private void Save()
    {
        columnSaveData.position = this.transform.position;
        columnSaveData.rotation = this.transform.rotation;
        columnSaveData.title = title;
        json = JsonUtility.ToJson(columnSaveData);
        Debug.Log(json);
        File.AppendAllText(Application.dataPath + "/saveFile.json", 'C' + json + "\n");
    }

    private void Load()
    {
        this.transform.position = columnSaveData.position;
        this.transform.rotation = columnSaveData.rotation;
        ChangeInputField("", columnSaveData.title);
    }

    public void Load(string jsonString)
    {
        ColumnSaveData loadedColumnSaveData = JsonUtility.FromJson<ColumnSaveData>(jsonString);

        this.transform.position = loadedColumnSaveData.position;
        this.transform.rotation = loadedColumnSaveData.rotation;
        ChangeInputField("", loadedColumnSaveData.title);
    }

    [ClientRpc]
    private void OnDeleteColumn()
    {
        SetIsEditingColumn(false);
    }

    struct ColumnSaveData
    {
        public Vector3 position;
        public Quaternion rotation;
        public string title;

        public void print()
        {
            Debug.Log(position);
            Debug.Log(rotation);
            Debug.Log(title);
        }
    }

    #endregion
}
