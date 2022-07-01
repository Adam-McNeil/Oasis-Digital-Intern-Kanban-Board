using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerController : NetworkBehaviour
{
    [Header("Adjustible Variables ")]
    [SerializeField] private float speed;
    [SerializeField] private float sensitivity;
    private CharacterController characterController;
    [Header("Child Refences")]
    [SerializeField] private Camera myCamera;
    private float xRotation = 0;
    private float yRotation = 0;

    private void Start()
    {
        gameObject.transform.position = new Vector3(0,2,0);
        Cursor.lockState = CursorLockMode.Locked;
        characterController = GetComponent<CharacterController>();

        if (isLocalPlayer)
        {
            myCamera.gameObject.SetActive(true);
        }
    }

    private void Update()
    {
        if (isLocalPlayer)
        {
            DoMovement();
            RotateCamera();
        }
    }

    void DoMovement()
    {
        Vector3 motion = (transform.right * Input.GetAxisRaw("Horizontal") + transform.forward * Input.GetAxisRaw("Vertical")).normalized * speed;
        characterController.Move(motion);
    }

    void RotateCamera()
    {
        float xMouseChange = Input.GetAxis("Mouse X") * Time.deltaTime * sensitivity;
        float yMouseChange = Input.GetAxis("Mouse Y") * Time.deltaTime * sensitivity;

        Debug.Log(xMouseChange);
        Debug.Log(yMouseChange);

        yRotation += xMouseChange;
        xRotation -= yMouseChange;

        xRotation = Mathf.Clamp(xRotation, -90, 90);

        transform.rotation = Quaternion.Euler(0, yRotation, 0);
        myCamera.transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
    }
}
