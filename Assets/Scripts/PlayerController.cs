using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerController : NetworkBehaviour
{
    private CharacterController characterController;
    private bool isGamePaused;
    [HideInInspector]
    public bool isEditing;
    private Camera myCamera;
    private CameraFollow myCameraScript;
    private float xRotation = 0;
    private float yRotation = 0;

    [Header("Adjustible Variables ")]
    [SerializeField] private float speed;
    [SerializeField] private float sensitivity;

    [Header("Children Refences")]
    [SerializeField] private Transform cameraOffset;


    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        characterController = GetComponent<CharacterController>();
        if (isLocalPlayer)
        {
            gameObject.tag = "Local Player";
            myCamera = GameObject.Find("Camera").GetComponent<Camera>();
            myCameraScript = myCamera.GetComponent<CameraFollow>();
        }
        else
        {
            gameObject.tag = "Nonlocal Player";
        }
    }

    private void Update()
    {
        if (isLocalPlayer)
        {
            Escape();
            if (!isGamePaused && !isEditing)
            {
                DoMovement();
                RotateCamera();
                myCameraScript.UpdateGoalPosition(cameraOffset.position);
            }
        }
    }

#region Movement
    void DoMovement()
    {
        Vector3 motion = (transform.right * Input.GetAxisRaw("Horizontal") + transform.forward * Input.GetAxisRaw("Vertical")).normalized * speed;
        characterController.Move(motion);
    }

    void RotateCamera()
    {
        float xMouseChange = Input.GetAxis("Mouse X") * Time.deltaTime * sensitivity;
        float yMouseChange = Input.GetAxis("Mouse Y") * Time.deltaTime * sensitivity;

        yRotation += xMouseChange;
        xRotation -= yMouseChange;

        xRotation = Mathf.Clamp(xRotation, -90, 90);

        transform.rotation = Quaternion.Euler(0, yRotation, 0);
        myCamera.transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
    }

#endregion

#region Pause
    private void Escape()
    {
        if (Input.GetKeyDown(KeyCode.Tab) && !isEditing)
        {
            if (isGamePaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }            
        }
    }

    public void Pause()
    {
        Cursor.lockState = CursorLockMode.None;
        isGamePaused = true;
    }

    public void Resume()
    {
        Cursor.lockState = CursorLockMode.Locked;
        isGamePaused = false;
    }
    #endregion

}
