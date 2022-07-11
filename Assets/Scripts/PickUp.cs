using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.XR.OpenVR;
using UnityEngine.InputSystem;

public class PickUp : MonoBehaviour
{
    [Header("PickUp Settings")]
    [SerializeField] private Transform holdArea;
    [SerializeField] private float pickRange = 5f; 
    [SerializeField] private float pickUpForce = 150f;
    [SerializeField] private float dragResistance = 10f;
    [SerializeField] private float throwForce = 10f;

    [SerializeField] private InputActionReference grabAction;
    [SerializeField] private InputActionReference throwAction;

    private GameObject heldObject;
    private Material heldObjectMaterial;
    private Color currentColor;
    private Rigidbody heldObjectRB;


    private void Update() {

        if(Input.GetMouseButtonDown(0)){
            if(heldObject == null)
            {
                RaycastHit hit;

                if(Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, pickRange))
                {
                    if(hit.transform.gameObject.tag == "Ticket")
                    {
                        PickUpObject(hit.transform.gameObject);
                    }
        
                }
            }
            else
            {
                DropObject();
            }
        }

        if(heldObject != null){
            MoveObject();
            if(Input.GetMouseButtonDown(1)){
                ThrowObject();
            }
        }
        
    //grabAction.action.performed += attemptGrab;
    //throwAction.action.performed += attemptThrow;
    }


    private void attemptThrow(InputAction.CallbackContext obj)
    {
      ThrowObject();
    }
    private void attemptGrab(InputAction.CallbackContext obj)
    {
        if(heldObject == null)
        {
            RaycastHit hit;

            if(Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, pickRange))
            {
                if(hit.transform.gameObject.tag == "Ticket")
                {
                    PickUpObject(hit.transform.gameObject);
                }
                    
            }
        }
        else
        {
            DropObject();
        }
    }

    void PickUpObject(GameObject pickedObject)
    {
        if(pickedObject.GetComponent<Rigidbody>())
        {  
            heldObjectRB = pickedObject.GetComponent<Rigidbody>();
            heldObjectRB.useGravity = false;
            heldObjectRB.drag = dragResistance;
            heldObjectRB.constraints = RigidbodyConstraints.FreezeRotation;

            heldObjectRB.transform.parent = holdArea; 
            heldObject = pickedObject;
        }
        
    }

    void DropObject()
    {
        heldObjectRB.useGravity = true;
        heldObjectRB.drag = 0;
        heldObjectRB.constraints = RigidbodyConstraints.None;

        heldObject.transform.parent = null; 
        heldObject = null;
    }

    void ThrowObject()
    {
        heldObjectRB.useGravity = true;
        heldObjectRB.drag = 0;
        heldObjectRB.constraints = RigidbodyConstraints.None;
        heldObjectRB.velocity = (holdArea.transform.forward * throwForce);

        heldObject.transform.parent = null; 
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
