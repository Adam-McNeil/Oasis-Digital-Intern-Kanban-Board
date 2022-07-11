using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PickUp : NetworkBehaviour
{
    [Header("PickUp Settings")]
    [SerializeField] private GameObject playerCamera;
    [SerializeField] private Transform holdArea;
    [SerializeField] private float pickRange = 5f; 
    [SerializeField] private float pickUpForce = 150f;
    [SerializeField] private float dragResistance = 10f;
    [SerializeField] private float throwForce = 10f;

    private GameObject heldObject;
    private Material heldObjectMaterial;
    private Color currentColor;
    private Rigidbody heldObjectRB;
    private bool isHoldingObject;


    private void Update() {
        if (isLocalPlayer)
        {
            if(Input.GetMouseButtonDown(0))
            {
                if (!isHoldingObject)
                {
                    RaycastHit hit;
                    if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, pickRange))
                    {
                        GameObject hitObject = hit.transform.gameObject;
                        if (hitObject.CompareTag("Ticket"))
                        {
                            PickUpObjectCmd(hitObject.GetComponent<NetworkIdentity>().netId);
                            isHoldingObject = true;
                        }
                    }
                }
                else
                {
                    DropObjectCmd();
                    isHoldingObject = false;

                }
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
    }

    void PickUpObject(GameObject pickedObject)
    {
        
        heldObjectRB = pickedObject.GetComponent<Rigidbody>();
        heldObjectRB.useGravity = false;
        heldObjectRB.drag = dragResistance;
        heldObjectRB.constraints = RigidbodyConstraints.FreezeRotation;
        //heldObjectRB.transform.parent = holdArea; 
        heldObject = pickedObject;
    }


    [Command]
    void DropObjectCmd()
    {
        DropObject();
    }

    void DropObject()
    {
        heldObjectRB.useGravity = true;
        heldObjectRB.drag = 0;
        heldObjectRB.constraints = RigidbodyConstraints.None;
        //heldObject.transform.parent = null; 
        heldObject = null;
    }

    [Command]
    void ThrowObjectCmd()
    {
        ThrowObject();
    }

    void ThrowObject()
    {
        heldObjectRB.useGravity = true;
        heldObjectRB.drag = 0;
        heldObjectRB.constraints = RigidbodyConstraints.None;
        heldObjectRB.velocity = (holdArea.transform.forward * throwForce);
        //heldObject.transform.parent = null; 
        heldObject = null;

    }

    void MoveObject()
    {
        if(Vector3.Distance(heldObject.transform.position, holdArea.position) > 0.1f)
        {
            Vector3 moveDirection = (holdArea.position - heldObject.transform.position);
            heldObjectRB.AddForce(moveDirection * pickUpForce);
        }
    }
}
