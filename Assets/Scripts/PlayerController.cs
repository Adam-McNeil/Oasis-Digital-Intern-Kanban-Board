using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using TMPro;

public class PlayerControl : NetworkBehaviour
{
    static public GameObject localPlayerCamera;
    public float speed = 7.5f;
    public float jumpSpeed = 12f;
    public float gravity = 9.81f;
    public float lookXLimit = 45.0f;
    Vector3 moveDirection = Vector3.zero;
    float rotationX = 0;
    float rotationY = 0;

    private CharacterController characterController;
    static public bool isGamePaused;
    [HideInInspector]
    public bool isEditing;

    [SerializeField] private float sensitivity = 30;
    public CharacterController playerController;

    [Header("Children Refences")]
    [SerializeField] private Transform cameraOffset;
    [SerializeField] private GameObject desktopCamera;
    [SerializeField] private TextMeshProUGUI usernameText;

    private TMP_InputField usernameInputField;
    private string usernameSyncVar;

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

    void RotationCamera()
    {
          rotationX += -Input.GetAxis("Mouse Y") * sensitivity;
          rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
          desktopCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
          transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * sensitivity, 0);
    }

    #endregion

}
