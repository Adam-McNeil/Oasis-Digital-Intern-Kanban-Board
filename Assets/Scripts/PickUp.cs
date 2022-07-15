using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.XR.OpenVR;
using UnityEngine.InputSystem;
using Mirror;

public class PickUp : NetworkBehaviour
{

    [Header("PickUp Settings")]
    private GameObject playerCamera;
    [SerializeField] private GameObject holdArea;
    [SerializeField] private float pickRange = 5f;
    [SerializeField] private float pickUpForce = 150f;
    [Range(0f, 1f)]
    [SerializeField] private float alphaPickUp = .8f;
    [SerializeField] private float dragResistance = 10f;
    [SerializeField] private float throwForce = 10f;
    [SerializeField] private float slerpSpeed = 10f;

    [SerializeField] private InputActionReference grabAction;
    [SerializeField] private InputActionReference throwAction;
    [SerializeField] private Material tempMaterial;

    private GameObject heldObject;
    private Material heldObjectMaterial;
    private Material orginalObjectMaterial;
    private Color currentColor;
    private Rigidbody heldObjectRB;
    private bool isHoldingObject;

    private float offset = 2.5f;
    private float minOffset = 1;
    private float maxOffset = 10;
    private float offsetSensitivity = 500;



    private void Start()
    {
        if (isLocalPlayer)
        {
            holdArea.AddComponent<FollowCamera>();
            holdArea.GetComponent<FollowCamera>().SetActiveCamera(playerCamera);
        }
    }

    private void Update()
    {
        if (isLocalPlayer)
        {
            UpdateOffset();
            grabAction.action.performed += attemptGrab;
            throwAction.action.performed += attemptThrow;

            if (Input.GetMouseButtonUp(0))
            {
                attemptGrab();
            }
            if (Input.GetMouseButtonDown(1) && isHoldingObject)
            {
                ThrowObjectCmd();
                isHoldingObject = false;
            }
        }
        if (heldObject != null && isServer)
        {
            MoveObject();
        }
    }

    private void attemptThrow(InputAction.CallbackContext obj)
    {
        if (isHoldingObject)
        {
            ThrowObjectCmd();
            isHoldingObject = false;
        }
    }

    private void attemptGrab(InputAction.CallbackContext obj)
    {
        if (!isHoldingObject)
        {
            RaycastHit hit;
            Debug.DrawLine(playerCamera.transform.position, playerCamera.transform.position + playerCamera.transform.forward * 5, new Color(0, 0, 0), 3f);

            if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, pickRange))
            {
                if (hit.transform.gameObject.tag == "Ticket" || hit.transform.gameObject.tag == "hammer")
                {
                    isHoldingObject = true;
                    PickUpObjectCmd(hit.transform.gameObject.GetComponent<NetworkIdentity>().netId);
                }

            }
        }
        else
        {
            DropObjectCmd();
            isHoldingObject = false;
        }
    }

    private void attemptGrab()
    {
        if (!isHoldingObject)
        {
            RaycastHit hit;
            Debug.DrawLine(playerCamera.transform.position, playerCamera.transform.position + playerCamera.transform.forward * 5, new Color(0, 0, 0), 3f);

            if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, pickRange))
            {
                if (hit.transform.gameObject.tag == "Ticket" || hit.transform.gameObject.tag == "hammer")
                {
                    isHoldingObject = true;
                    PickUpObjectCmd(hit.transform.gameObject.GetComponent<NetworkIdentity>().netId);
                }
            }
        }
        else
        {
            DropObjectCmd();
            isHoldingObject = false;
        }
    }

    [Command]
    void PickUpObjectCmd(uint ID)
    {
        GameObject[] ticketArray = GameObject.FindGameObjectsWithTag("Ticket");
        foreach (GameObject ticket in ticketArray)
        {
            if (ticket.GetComponent<NetworkIdentity>().netId == ID)
            {
                PickUpObject(ticket);
                return;
            }
        }
        GameObject[] hammerArray = GameObject.FindGameObjectsWithTag("hammer");
        foreach (GameObject hammer in hammerArray)
        {
          if (hammer.GetComponent<NetworkIdentity>().netId == ID)
          {
            PickUpObject(hammer);
            return;
          }
        }
    }

    void PickUpObject(GameObject pickedObject)
    {
        if (pickedObject.CompareTag("Ticket"))
        {
            pickedObject.GetComponent<Animator>().Play("Ticket_Shrink");
        }
        
        orginalObjectMaterial = pickedObject.GetComponent<Renderer>().material;
        currentColor = orginalObjectMaterial.color;
        Color alteredAlpha = currentColor;
        alteredAlpha.a = alphaPickUp;
        tempMaterial.color = alteredAlpha;

        heldObjectRB = pickedObject.GetComponent<Rigidbody>();
        heldObjectRB.useGravity = false;
        heldObjectRB.drag = dragResistance;
        heldObjectRB.constraints = RigidbodyConstraints.FreezeRotation; 
        heldObject = pickedObject;

        heldObject.GetComponent<Renderer>().material = tempMaterial;
    }

    [Command]
    void DropObjectCmd()
    {
        DropObject();
    }

    void DropObject()
    {
        heldObject.GetComponent<Renderer>().material = orginalObjectMaterial;

        heldObjectRB.useGravity = true;
        heldObjectRB.drag = 0;
        heldObjectRB.constraints = RigidbodyConstraints.None;
        heldObject = null;
    }

    [Command]
    void ThrowObjectCmd()
    {
        ThrowObject();
    }

    void ThrowObject()
    {
        heldObject.GetComponent<Renderer>().material = orginalObjectMaterial;

        heldObjectRB.useGravity = true;
        heldObjectRB.drag = 0;
        heldObjectRB.constraints = RigidbodyConstraints.None;
        heldObjectRB.velocity = (holdArea.transform.forward * throwForce);
        //heldObject.transform.parent = null; 
        heldObject = null;
    }

    void MoveObject()
    {
        Vector3 goalPosition = holdArea.transform.position + holdArea.transform.forward * offset;
        Vector3 changeInPosition = Vector3.Slerp(heldObject.transform.position, goalPosition, slerpSpeed * Time.deltaTime) - heldObject.transform.position;
        heldObjectRB.AddForce(changeInPosition * pickUpForce);
    }

    public void SetActiveCamera(GameObject camera)
    {
        playerCamera = camera;
    }

    private void UpdateOffset()
    {
        float mouseChange = Input.GetAxis("Mouse ScrollWheel");
        if (mouseChange != 0)
        {
            offset += mouseChange * Time.deltaTime * offsetSensitivity;
            offset = Mathf.Clamp(offset, minOffset, maxOffset);
            UpdateOffsetCmd(offset);
        }
    }

    [Command]
    private void UpdateOffsetCmd(float newOffset)
    {
        offset = newOffset;
    }

}
