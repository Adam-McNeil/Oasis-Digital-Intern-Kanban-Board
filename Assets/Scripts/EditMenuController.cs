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

    private bool editModeEnabled = false;
    private bool oldEditModeEnabled = false;

    public GameObject activeEditTable;
    public GameObject targetedTicket;
    private GameObject activeCamera;

    private Vector3 smallScale = new Vector3(.1f, .1f, .1f);
    private Vector3 largeScale = new Vector3(1, 1, 1);
    private float slerpSpeed = 0.5f;

    private AudioSource audioSource;
    [SerializeField] private AudioClip soundEffect;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        editModeEnabled = PlayerController.isEditing;
        if (editModeEnabled != oldEditModeEnabled)
        {
            if (editModeEnabled)
            {
                audioSource.PlayOneShot(soundEffect);
                Cursor.lockState = CursorLockMode.None;
                Transform localPlayerTransform = activeCamera.transform;
                this.transform.localScale = smallScale;
                this.transform.position = localPlayerTransform.position + localPlayerTransform.forward * offset;
                this.transform.LookAt(localPlayerTransform);
                this.transform.Rotate(0, 180, 0);

            }
            else
            {
                this.transform.position = farAway;
            }
        }
        oldEditModeEnabled = editModeEnabled;
    }
    private void FixedUpdate()
    {
        this.transform.localScale = Vector3.Slerp(this.transform.localScale, largeScale, slerpSpeed);
    }

    public void SetActiveCamera(GameObject camera)
    {
        this.transform.position = farAway;
        activeCamera = camera;
        GetComponentInChildren<Canvas>().worldCamera = activeCamera.GetComponent<Camera>();
    }

    public void OnButtonSumbit()
    {
        activeEditTable.GetComponent<EditTable>().SubmitEditChanges();
    }
}

