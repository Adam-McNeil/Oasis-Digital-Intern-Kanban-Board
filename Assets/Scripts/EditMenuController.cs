using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EditMenuController : MonoBehaviour
{
    //[SerializeField] private GameObject editmenu;
    [SerializeField] private float offset;
    [SerializeField] private Vector3 farAway;
    [SerializeField] public TMP_InputField headerInputField;
    [SerializeField] public TMP_InputField detailInputField;
    [SerializeField] public TMP_Dropdown assignedDropDown;
    [SerializeField] public TMP_Dropdown colorDropDown;
    [SerializeField] public TMP_Text currentTicketText;

    private PlayerController playerControllerScript;
    private EditTable editTableScript;
    private bool editModeEnabled = false;

    private GameObject activeEditTable;
    public GameObject targetedTicket;
    private GameObject activeCamera;

    private void Update()
    {
        editModeEnabled = PlayerController.isEditing;
        
        if(editModeEnabled){
            Cursor.lockState = CursorLockMode.None;
            Transform localPlayerTransform = activeCamera.transform;
            this.transform.position = localPlayerTransform.position + localPlayerTransform.forward * offset;
            this.transform.LookAt(localPlayerTransform);
            this.transform.Rotate(0, 180, 0);

        }else{
            this.transform.position = farAway;
        }
    }

    public void SetActiveCamera(GameObject camera)
    {
        this.transform.position = farAway;
        activeCamera = camera;
        GetComponentInChildren<Canvas>().worldCamera = activeCamera.GetComponent<Camera>();
    }
}

