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
    [SerializeField] private float pickRange = 50f;
    [SerializeField] private float pickUpForce = 150f;
    [SerializeField] private float dragResistance = 20f;
    [SerializeField] private float throwForce = 10f;
    [SerializeField] private float slerpSpeed = 10f;

    [SerializeField] private InputActionReference grabAction;
    [SerializeField] private InputActionReference throwAction;
    [SerializeField] private Material tempMaterial;

    private GameObject heldObject;
    private List<OutlineNetworkController> outlineNetworkControllers = new List<OutlineNetworkController>();
    private OutlineNetworkController outlinedGameObject;
    private OutlineNetworkController lastOutlinedGameObject;
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
        if (isLocalPlayer && !PlayerController.isEditing)
        {
            UpdateOffset();
            grabAction.action.performed += attemptGrab;
            throwAction.action.performed += attemptThrow;

            attemptGrab();
            
            if (Input.GetMouseButtonDown(1) && isHoldingObject)
            {
                ThrowObjectCmd();
                if (isClientOnly)
                    ThrowObjectLocal();
                
            }
        }
    }

    private void FixedUpdate()
    {
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

        RaycastHit hit;
        GameObject hitObject = null;
        bool ticketHit = false;

        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, pickRange))
        {
            hitObject = hit.transform.gameObject;
            if (hitObject.CompareTag("Ticket"))
            {
                outlinedGameObject = hitObject.GetComponent<OutlineNetworkController>();
                if (!outlineNetworkControllers.Contains(outlinedGameObject))
                {
                    outlineNetworkControllers.Add(outlinedGameObject);
                    if (outlineNetworkControllers.Count > 1)
                    {
                        outlineNetworkControllers[0].UpdateOutlineLocal(false);
                        outlineNetworkControllers.RemoveAt(0);
                    }
                    outlinedGameObject.UpdateOutlineLocal(true);
                }
                ticketHit = true;
            }
            else
            {
                RemoveAllOutlines();
            }
        }
        else
        {
            RemoveAllOutlines();
        }

        if (Input.GetMouseButtonDown(0))
        {
            if (!isHoldingObject && ticketHit)
            {
                PickUpObjectCmd(hitObject.GetComponent<NetworkIdentity>().netId);
                if (isClientOnly)
                    PickUpObjectLocal(hitObject);
            }
            else
            {
                DropObjectCmd();
                if (isClientOnly)
                    DropObjectLocal();
            }
        }
    }

    private void attemptGrab()
    {

        RaycastHit hit;
        GameObject hitObject = null;
        bool ticketHit = false;

        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, pickRange))
        {
            hitObject = hit.transform.gameObject;
            if (hitObject.CompareTag("Ticket"))
            {
                outlinedGameObject = hitObject.GetComponent<OutlineNetworkController>();
                if (!outlineNetworkControllers.Contains(outlinedGameObject))
                {
                    outlineNetworkControllers.Add(outlinedGameObject);
                    if (outlineNetworkControllers.Count > 1)
                    {
                        outlineNetworkControllers[0].UpdateOutlineLocal(false);
                        outlineNetworkControllers.RemoveAt(0);
                    }
                    outlinedGameObject.UpdateOutlineLocal(true);
                }
                ticketHit = true;
            }
            else
            {
                RemoveAllOutlines();
            }
        }
        else
        {
            RemoveAllOutlines();
        }

        if (Input.GetMouseButtonDown(0))
        {
            if (!isHoldingObject && ticketHit)
            {
                PickUpObjectCmd(hitObject.GetComponent<NetworkIdentity>().netId);
                if (isClientOnly)
                    PickUpObjectLocal(hitObject);
            }
            else
            {
                DropObjectCmd();
                if (isClientOnly)
                    DropObjectLocal();
            }
        }
    }

    private void RemoveAllOutlines()
    {
        foreach (OutlineNetworkController outlineNetworkController in outlineNetworkControllers)
        {
            outlineNetworkControllers[0].UpdateOutlineLocal(false);
        }
        outlineNetworkControllers.Clear();
    }

    #region PickUp
    [Command]
    void PickUpObjectCmd(uint ID)
    {
        foreach (NetworkIdentity ticket in TicketData.networkIdentities)
        {
            if (ticket.netId == ID)
            {
                PickUpObject(ticket.gameObject);
                return;
            }
        }
    }

    void PickUpObjectLocal(GameObject pickedUpGameObject)
    {
        pickedUpGameObject.GetComponent<OutlineNetworkController>().UpdateOutlineLocal(true, true);
        heldObject = pickedUpGameObject;
        isHoldingObject = true;
    }

    void PickUpObject(GameObject pickedObject)
    {
        if (pickedObject.CompareTag("Ticket"))
        {
            pickedObject.GetComponent<Animator>().Play("Ticket_Shrink");
        }
        heldObjectRB = pickedObject.GetComponent<Rigidbody>();
        heldObjectRB.useGravity = false;
        heldObjectRB.drag = dragResistance;
        heldObjectRB.constraints = RigidbodyConstraints.FreezeRotation; 
        heldObject = pickedObject;
        isHoldingObject = true;
        pickedObject.GetComponent<OutlineNetworkController>().UpdateOutlineCmd(true, true);

    }
    #endregion

    #region Drop
    [Command]
    void DropObjectCmd()
    {
        DropObject();
    }

    void DropObjectLocal()
    {
        if (heldObject != null)
        {
            heldObject.GetComponent<OutlineNetworkController>().UpdateOutlineCmd(false, true);
            heldObject = null;
            isHoldingObject = false;
        }
    }

    void DropObject()
    {
        if (heldObject != null)
        {
            heldObjectRB.useGravity = true;
            heldObjectRB.drag = 0;
            heldObjectRB.constraints = RigidbodyConstraints.None;
            heldObject.GetComponent<OutlineNetworkController>().UpdateOutlineCmd(false, true);
            heldObject = null;
            isHoldingObject = false;

        }
    }
    #endregion

    #region Throw
    [Command]
    void ThrowObjectCmd()
    {
        ThrowObject();
    }

    private void ThrowObjectLocal()
    {
        if (heldObject != null)
        {
            heldObject.GetComponent<OutlineNetworkController>().UpdateOutlineCmd(false, true);
            heldObject = null;
            isHoldingObject = false;
        }
    }

    void ThrowObject()
    {
        if (heldObject != null)
        {
            heldObjectRB.useGravity = true;
            heldObjectRB.drag = 0;
            heldObjectRB.constraints = RigidbodyConstraints.None;
            heldObjectRB.velocity = (holdArea.transform.forward * throwForce);
            heldObject.GetComponent<OutlineNetworkController>().UpdateOutlineCmd(false, true);
            heldObject = null;
            isHoldingObject = false;

        }
    }
    #endregion

    void MoveObject()
    {
        Vector3 goalPosition = holdArea.transform.position + holdArea.transform.forward * offset;
        Vector3 changeInPosition = Vector3.Slerp(heldObject.transform.position, goalPosition, slerpSpeed) - heldObject.transform.position;
        heldObjectRB.AddForce(changeInPosition * pickUpForce);
    }

    #region Offset
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
    #endregion

    public void SetActiveCamera(GameObject camera)
    {
        playerCamera = camera;
    }

}
