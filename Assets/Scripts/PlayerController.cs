using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerController : NetworkBehaviour
{
    public float speed = 7.5f;
    public float jumpSpeed = 12f;
    public float gravity = 9.81f;
    public Camera playerCamera;
    public float lookXLimit = 45.0f;
    Vector3 moveDirection = Vector3.zero;
    float rotationX = 0;

    private CharacterController characterController;
    private bool isGamePaused;
    [HideInInspector]
    public bool isEditing;

    [SerializeField] private float sensitivity = 30;

    public CharacterController playerController;

    [Header("Children Refences")]
    [SerializeField] private Transform cameraOffset;
    [SerializeField] private GameObject desktopCamera;


    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        characterController = GetComponent<CharacterController>();
        if (isLocalPlayer)
        {
            gameObject.tag = "Local Player";
            //desktopCamera.gameObject.SetActive(true);
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
                playerMovement();
                RotateCamera();
            }
        }
    }

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

    #region Movement
    private void playerMovement()
    {

        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);

        float curSpeedX =  speed * Input.GetAxis("Vertical");
        float curSpeedY =  speed * Input.GetAxis("Horizontal");
        float movementDirectionY = moveDirection.y;
        moveDirection = (forward * curSpeedX) + (right * curSpeedY);

        if (Input.GetButton("Jump") && characterController.isGrounded)
        {
            moveDirection.y = jumpSpeed;
        }
        else
        {
            moveDirection.y = movementDirectionY;
        }

        if (!characterController.isGrounded)
        {
            moveDirection.y -= gravity * Time.deltaTime;
        }

        characterController.Move(moveDirection * Time.deltaTime);
    }

    void RotateCamera()
    {
        rotationX += -Input.GetAxis("Mouse Y") * sensitivity;
        rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
        playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
        transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * sensitivity, 0);
    }

    #endregion
}
