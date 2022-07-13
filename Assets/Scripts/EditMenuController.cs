using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EditMenuController : MonoBehaviour
{
    //[SerializeField] private GameObject editmenu;
    [SerializeField] private float offset;
    [SerializeField] private Vector3 farAway;
    [SerializeField] private TMP_InputField headerInputField;
    [SerializeField] private TMP_InputField detailInputField;
    private bool editModeEnabled = false;
    private GameObject activeCamera;

    private void Update()
    {
        editModeEnabled = PlayerController.isEditing;
        if(editModeEnabled){
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

